using cloudscribe.SimpleContactForm.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Components
{
    public class ConfigContactFormResolver : IContactFormResolver
    {
        public ConfigContactFormResolver(
            IOptions<ContactFormSettings> contactFormAccessor
            )
        {
            form = contactFormAccessor.Value;
        }

        private ContactFormSettings form;

        public Task<ContactFormSettings> GetCurrentContactForm()
        {
            return Task.FromResult(form);
        }
    }
}
