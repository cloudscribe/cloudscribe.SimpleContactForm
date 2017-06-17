// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2016-11-19
// 

using cloudscribe.SimpleContactForm.Components;
using cloudscribe.SimpleContactForm.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using cloudscribe.Messaging.Email;
using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using cloudscribe.Web.Common.Components;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeSimpleContactForm(
            this IServiceCollection services,
            IConfigurationRoot configuration,
            bool includeSmtpMessageProcessor = true)
        {

            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
            services.Configure<RecaptchaKeys>(configuration.GetSection("RecaptchaKeys"));
            services.Configure<SmtpMessageProcessorOptions>(configuration.GetSection("SmtpMessageProcessorOptions"));
            services.Configure<ContactFormSettings>(configuration.GetSection("ContactFormSettings"));


            services.TryAddScoped<ISmtpOptionsProvider, ConfigSmtpOptionsProvider>();
            services.TryAddScoped<IRecaptchaKeysProvider, ConfigRecaptchaKeysProvider>();
            services.AddScoped<ContactFormService, ContactFormService>();
            
            services.TryAddScoped<IContactFormResolver, ConfigContactFormResolver>();
            // pass in false if you want to implement custom logic for notification
            // instead of the built in logic
            // there can be multiple registered IProcessMessages implementations and all of them will be invoked
            // for example could implement one to persist the messages in data storage
            // could be one to integrate with salesforce.com or some other crm
            if (includeSmtpMessageProcessor)
            {
                services.AddScoped<IProcessMessages, SmtpMessageProcessor>();
            }
            


            return services;
        }

        [Obsolete("AddEmbeddedViewsForCloudscribeSimpleContactForm is deprecated, please use AddCloudscribeSimpleContactFormViews instead.")]
        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeSimpleContactForm(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(ContactFormService).GetTypeInfo().Assembly,
                    "cloudscribe.SimpleContactForm"
                ));

            return options;
        }

        public static RazorViewEngineOptions AddCloudscribeSimpleContactFormViews(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(ContactFormService).GetTypeInfo().Assembly,
                    "cloudscribe.SimpleContactForm"
                ));

            return options;
        }
    }
}
