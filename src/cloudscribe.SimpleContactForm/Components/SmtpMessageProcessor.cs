// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2016-11-20
// 

using cloudscribe.Messaging.Email;
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
    public class SmtpMessageProcessor : IProcessMessages
    {
        public SmtpMessageProcessor(
            ViewRenderer viewRenderer,
            IContactFormResolver contactFormResolver,
            ISmtpOptionsProvider smtpOptionsProvider,
            IOptions<SmtpMessageProcessorOptions> messageProcessorOptionsAccessor,
            ILogger<SmtpMessageProcessor> logger
            )
        {
            this.viewRenderer = viewRenderer;
            this.smtpOptionsProvider = smtpOptionsProvider;
            this.contactFormResolver = contactFormResolver;
            messageProcessorOptions = messageProcessorOptionsAccessor.Value;
            log = logger;
        }

        private ViewRenderer viewRenderer;
        private ISmtpOptionsProvider smtpOptionsProvider;
        private IContactFormResolver contactFormResolver;
        private SmtpMessageProcessorOptions messageProcessorOptions;
        private ILogger log;


        public async Task Process(ContactFormMessage message)
        {
            var form = await contactFormResolver.GetCurrentContactForm().ConfigureAwait(false);
            var smtpOptions = await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);

            if(string.IsNullOrEmpty(smtpOptions.Server))
            {
                throw new InvalidOperationException("smtp settings are not configured");
            }

            EmailSender sender = new EmailSender();
           
            try
            {
                var plainTextMessage
                   = await viewRenderer.RenderViewAsString<ContactFormMessage>(messageProcessorOptions.NotificationTextViewName, message);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<ContactFormMessage>(messageProcessorOptions.NotificationHtmlViewName, message);

                var replyTo = message.Email;
                await sender.SendMultipleEmailAsync(
                    smtpOptions,
                    form.NotificationEmailCsv,
                    smtpOptions.DefaultEmailFromAddress,
                    form.NotificationEmailCsv,
                    plainTextMessage,
                    htmlMessage,
                    replyTo
                    ).ConfigureAwait(false);

                if (form.CopySubmitterEmailOnSubmission)
                {
                    try
                    {
                        plainTextMessage
                        = await viewRenderer.RenderViewAsString<ContactFormMessage>(
                            messageProcessorOptions.SubmitterNotificationTextViewName,
                            message);

                        htmlMessage
                            = await viewRenderer.RenderViewAsString<ContactFormMessage>(
                                messageProcessorOptions.SubmitterNotificationHtmlViewName,
                                message);

                        await sender.SendEmailAsync(
                            smtpOptions,
                            message.Email,
                            smtpOptions.DefaultEmailFromAddress,
                            form.NotificationEmailCsv,
                            plainTextMessage,
                            htmlMessage
                            ).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        log.LogError("error sending contact form submitter notification email", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                log.LogError("error sending contact form notification email: " + ex.Message, ex);
            }

            

        }
    }
}
