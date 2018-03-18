// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2018-03-18
// 

using cloudscribe.SimpleContactForm.Models;
using cloudscribe.SimpleContactForm.ViewModels;
using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Components
{
    public class ContactFormService
    {
        public ContactFormService(
            IEnumerable<IProcessContactForm> messageProcessors,
            IContactFormResolver contactFormResolver,
            IRecaptchaKeysProvider recaptchaKeysProvider,
            ILogger<ContactFormService> logger
            )
        {
            _contactFormResolver = contactFormResolver;
            _recaptchaKeys = recaptchaKeysProvider;
            _messageProcessors = messageProcessors;
            _log = logger;
        }

        private IContactFormResolver _contactFormResolver;
        private IRecaptchaKeysProvider _recaptchaKeys;
        private ContactFormSettings _form = null;
        private IEnumerable<IProcessContactForm> _messageProcessors;
        private ILogger _log;

        public async Task<ContactFormSettings> GetFormSettings()
        {
            if(_form == null)
            {
                _form = await _contactFormResolver.GetCurrentContactForm().ConfigureAwait(false);
            }

            return _form;
        }

        public async Task<bool> IsConfigured()
        {
            var form = await GetFormSettings().ConfigureAwait(false);
            if(string.IsNullOrEmpty(form.NotificationEmailCsv)) { return false; }

            //var smtpSettings = await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);
            //if (string.IsNullOrEmpty(smtpSettings.Server)) { return false; }

            return true;
        }

        public async Task<RecaptchaKeys> GetRecaptchaKeys()
        {
            return await _recaptchaKeys.GetKeys().ConfigureAwait(false);
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
            foreach(var processor in _messageProcessors)
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
                    _log.LogError("error processing contact form message", ex);
                    var me = new MessageError
                    {
                        Code = "processfailure",
                        Description = ex.Message
                    };
                    errorList.Add(me);
                }
            }

            if (successCount > 0) return MessageResult.Success;
            
            return MessageResult.Failed(errorList.ToArray());
        }
    }
}
