using cloudscribe.SimpleContactForm.CoreIntegration;
using cloudscribe.SimpleContactForm.Models;
using cloudscribe.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeSimpleContactFormCoreIntegration(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.TryAddScoped<ITenantResolver, CoreTenantResolver>();
            services.TryAddScoped<IContactFormResolver, SiteContactFormResolver>();
            services.TryAddScoped<IPrePopulateContactForm, FormPrepopulator>();
            services.AddScoped<IVersionProvider, VersionProvider>();
            services.AddScoped<IVersionProvider, cloudscribe.SimpleContactForm.VersionProvider>();

            return services;
        }

    }
}
