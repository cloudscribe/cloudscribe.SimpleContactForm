// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2018-03-18
// 

using cloudscribe.Email;
using cloudscribe.SimpleContactForm.Models;
using cloudscribe.Web.Common.Razor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Components
{
    public class ContactFormProcessor : IProcessContactForm
    {
        public ContactFormProcessor(
            ViewRenderer viewRenderer,
            ITenantResolver tenantResolver,
            IEmailSenderResolver emailSenderResolver,
            IContactFormResolver contactFormResolver,
            IOptions<ContactFormMessageOptions> messageProcessorOptionsAccessor,
            ILogger<ContactFormProcessor> logger
            )
        {
            _viewRenderer = viewRenderer;
            _tenantResolver = tenantResolver;
            _emailSenderResolver = emailSenderResolver;
            _contactFormResolver = contactFormResolver;
            _messageProcessorOptions = messageProcessorOptionsAccessor.Value;
            _log = logger;
        }

        private ViewRenderer _viewRenderer;
        private ITenantResolver _tenantResolver;
        private IEmailSenderResolver _emailSenderResolver;
        private IContactFormResolver _contactFormResolver;
        private ContactFormMessageOptions _messageProcessorOptions;
        private ILogger _log;


        public async Task<MessageResult> Process(ContactFormMessage message)
        {
            var errorList = new List<MessageError>();

            var sender = await _emailSenderResolver.GetEmailSender();
            if (sender == null)
            {
                var logMessage = $"failed to send account confirmation email because email settings are not populated";
                _log.LogError(logMessage);
                var m = new MessageError
                {
                    Code = "NoSenderError",
                    Description = logMessage
                };
                errorList.Add(m);
               
                return MessageResult.Failed(errorList.ToArray());
            }

            var form = await _contactFormResolver.GetCurrentContactForm().ConfigureAwait(false);
            message.Tenant = await _tenantResolver.ResolveTenant();
            
            try
            {
                var plainTextMessage
                   = await _viewRenderer.RenderViewAsString<ContactFormMessage>(_messageProcessorOptions.NotificationTextViewName, message);

                var htmlMessage
                    = await _viewRenderer.RenderViewAsString<ContactFormMessage>(_messageProcessorOptions.NotificationHtmlViewName, message);

                var replyTo = message.Email;
                await sender.SendEmailAsync(
                    form.NotificationEmailCsv,
                    null,
                    message.Subject,
                    plainTextMessage,
                    htmlMessage,
                    replyTo
                    ).ConfigureAwait(false);

                if (form.CopySubmitterEmailOnSubmission)
                {
                    try
                    {
                        plainTextMessage
                        = await _viewRenderer.RenderViewAsString<ContactFormMessage>(
                            _messageProcessorOptions.SubmitterNotificationTextViewName,
                            message);

                        htmlMessage
                            = await _viewRenderer.RenderViewAsString<ContactFormMessage>(
                                _messageProcessorOptions.SubmitterNotificationHtmlViewName,
                                message);

                        await sender.SendEmailAsync(
                            message.Email,
                            null,
                            message.Subject,
                            plainTextMessage,
                            htmlMessage
                            ).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError($"error sending contact form submitter notification email {ex.Message} : {ex.StackTrace}");
                        var m = new MessageError
                        {
                            Code = "SubmitterNotificationError",
                            Description = ex.Message
                        };
                        errorList.Add(m);
                    }

                }
            }
            catch (Exception ex)
            {
                _log.LogError($"error sending contact form notification email: {ex.Message} : {ex.StackTrace}");
                var m = new MessageError
                {
                    Code = "NotificationError",
                    Description = ex.Message
                };
                errorList.Add(m);
            }

            if(errorList.Count > 0)
            {
                return MessageResult.Failed(errorList.ToArray());
            }

            return MessageResult.Success;

        }
    }
}
