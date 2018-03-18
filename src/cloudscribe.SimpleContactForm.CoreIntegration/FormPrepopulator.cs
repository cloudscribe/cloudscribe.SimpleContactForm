using cloudscribe.Core.Models;
using cloudscribe.SimpleContactForm.Models;
using cloudscribe.SimpleContactForm.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.CoreIntegration
{
    public class FormPrepopulator : IPrePopulateContactForm
    {
        public FormPrepopulator(IUserContextResolver userResolver)
        {
            _userResolver = userResolver;
        }

        private IUserContextResolver _userResolver;

        public async Task Prepopulate(MessageViewModel model, ClaimsPrincipal user)
        {
            var siteUser = await _userResolver.GetCurrentUser();
            if(siteUser != null)
            {
                model.Email = siteUser.Email;
                if(!string.IsNullOrWhiteSpace(siteUser.FirstName) && !string.IsNullOrWhiteSpace(siteUser.LastName))
                {
                    model.Name = siteUser.FirstName + " " + siteUser.LastName;
                }
                else
                {
                    model.Name = siteUser.DisplayName;
                }
            }

        }


    }
}
