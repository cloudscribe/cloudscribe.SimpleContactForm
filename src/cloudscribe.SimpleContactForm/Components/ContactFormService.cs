// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2016-11-19
// 

using cloudscribe.Messaging.Email;
using cloudscribe.SimpleContactForm.Models;
using cloudscribe.SimpleContactForm.ViewModels;
using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Components
{
    public class ContactFormService
    {
        public ContactFormService(
            IEnumerable<IProcessMessages> messageProcessors,
            IContactFormResolver contactFormResolver,
            ISmtpOptionsProvider smtpOptionsProvider,
            IRecaptchaKeysProvider recaptchaKeysProvider,
            ILogger<ContactFormService> logger
            )
        {
            this.contactFormResolver = contactFormResolver;
            recaptchaKeys = recaptchaKeysProvider;
            this.messageProcessors = messageProcessors;
            this.smtpOptionsProvider = smtpOptionsProvider;
            log = logger;
        }

        private IContactFormResolver contactFormResolver;
        private IRecaptchaKeysProvider recaptchaKeys;
        private ContactFormSettings form = null;
        private ISmtpOptionsProvider smtpOptionsProvider;
        private IEnumerable<IProcessMessages> messageProcessors;
        private ILogger log;

        public async Task<ContactFormSettings> GetFormSettings()
        {
            if(form == null)
            {
                form = await contactFormResolver.GetCurrentContactForm().ConfigureAwait(false);
            }

            return form;
        }

        public async Task<bool> IsConfigured()
        {
            var form = await GetFormSettings().ConfigureAwait(false);
            if(string.IsNullOrEmpty(form.NotificationEmailCsv)) { return false; }

            var smtpSettings = await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);
            if (string.IsNullOrEmpty(smtpSettings.Server)) { return false; }

            return true;
        }

        public async Task<RecaptchaKeys> GetRecaptchaKeys()
        {
            return await recaptchaKeys.GetKeys().ConfigureAwait(false);
        }

        public async Task<MessageResult> ProcessMessage(MessageViewModel model, string ipAddress)
        {
            var message = ContactFormMessage.FromViewModel(model, ipAddress);
            if(string.IsNullOrEmpty(message.Subject))
            {
                var form = await GetFormSettings().ConfigureAwait(false);
                message.Subject = form.NotificationSubject;
            }

            int successCount = 0;
            int failureCount = 0;
            var errorList = new List<MessageError>();
            foreach(var processor in messageProcessors)
            {
                try
                {
                    var tempResult = await processor.Process(message).ConfigureAwait(false);
                    if(tempResult.Succeeded)
                    {
                        successCount += 1;
                    }
                    else
                    {
                        failureCount += 1;
                        errorList.AddRange(tempResult.Errors);
                    }
                    
                }
                catch(Exception ex)
                {
                    failureCount += 1;
                    log.LogError("error processing contact form message", ex);
                    var me = new MessageError();
                    me.Code = "processfailure";
                    me.Description = ex.Message;
                    errorList.Add(me);
                }
            }

            if (successCount > 0) return MessageResult.Success;
            
            return MessageResult.Failed(errorList.ToArray());
        }
    }
}
