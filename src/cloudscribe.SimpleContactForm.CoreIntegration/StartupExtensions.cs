using cloudscribe.SimpleContactForm.CoreIntegration;
using cloudscribe.SimpleContactForm.Models;
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

            return services;
        }

    }
}
