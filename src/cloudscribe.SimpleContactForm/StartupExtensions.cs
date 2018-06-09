// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2018-06-09
// 

using cloudscribe.SimpleContactForm.Components;
using cloudscribe.SimpleContactForm.Models;
using cloudscribe.Web.Common.Components;
using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeSimpleContactForm(
            this IServiceCollection services,
            IConfiguration configuration,
            bool includeDefaultMessageProcessor = true)
        {

            services.AddCloudscribeEmailSenders(configuration);
            services.Configure<RecaptchaKeys>(configuration.GetSection("RecaptchaKeys"));
            services.Configure<ContactFormMessageOptions>(configuration.GetSection("ContactFormMessageOptions"));
            services.Configure<ContactFormSettings>(configuration.GetSection("ContactFormSettings"));


           
            services.TryAddScoped<IRecaptchaKeysProvider, ConfigRecaptchaKeysProvider>();
            services.AddScoped<ContactFormService, ContactFormService>();
            
            services.TryAddScoped<IContactFormResolver, ConfigContactFormResolver>();
            services.TryAddScoped<ITenantResolver, NullTenantResolver>();
            services.TryAddScoped<IPrePopulateContactForm, NotImplementedContactFromPopulator>();
            // pass in false if you want to implement custom logic for notification
            // instead of the built in logic
            // there can be multiple registered IProcessMessages implementations and all of them will be invoked
            // for example could implement one to persist the messages in data storage
            // could be one to integrate with salesforce.com or some other crm
            if (includeDefaultMessageProcessor)
            {
                services.AddScoped<IProcessContactForm, ContactFormProcessor>();
            }
            


            return services;
        }

        //[Obsolete("AddEmbeddedViewsForCloudscribeSimpleContactForm is deprecated, please use AddCloudscribeSimpleContactFormViews instead.")]
        //public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeSimpleContactForm(this RazorViewEngineOptions options)
        //{
        //    options.FileProviders.Add(new EmbeddedFileProvider(
        //            typeof(ContactFormService).GetTypeInfo().Assembly,
        //            "cloudscribe.SimpleContactForm"
        //        ));

        //    return options;
        //}

        //public static RazorViewEngineOptions AddCloudscribeSimpleContactFormViews(this RazorViewEngineOptions options)
        //{
        //    options.FileProviders.Add(new EmbeddedFileProvider(
        //            typeof(ContactFormService).GetTypeInfo().Assembly,
        //            "cloudscribe.SimpleContactForm"
        //        ));

        //    return options;
        //}
    }
}
