using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Models
{
    public interface ITenantResolver
    {
        Task<object> ResolveTenant();
    }

    public class NullTenantResolver : ITenantResolver
    {
        public Task<object> ResolveTenant()
        {
            object obj = null;
            return Task.FromResult(obj);
        }
    }
}
