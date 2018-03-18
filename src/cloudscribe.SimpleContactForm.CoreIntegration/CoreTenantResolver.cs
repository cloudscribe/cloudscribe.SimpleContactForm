using cloudscribe.Core.Models;
using cloudscribe.SimpleContactForm.Models;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.CoreIntegration
{
    public class CoreTenantResolver : ITenantResolver
    {
        public CoreTenantResolver(
            SiteContext currentSite)
        {
            _currentSite = currentSite;
        }

        private SiteContext _currentSite;

        public Task<object> ResolveTenant()
        {
            return Task.FromResult(_currentSite as object);
        }
    }
}
