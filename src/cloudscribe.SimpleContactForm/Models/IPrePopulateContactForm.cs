using cloudscribe.SimpleContactForm.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Models
{
    public interface IPrePopulateContactForm
    {
        Task Prepopulate(MessageViewModel model, ClaimsPrincipal user);
    }

    public class NotImplementedContactFromPopulator : IPrePopulateContactForm
    {
        public Task Prepopulate(MessageViewModel model, ClaimsPrincipal user)
        {
            //do nothing
            return Task.CompletedTask;
        }
    }
}
