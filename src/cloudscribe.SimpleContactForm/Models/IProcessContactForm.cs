using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Models
{
    public interface IProcessContactForm
    {
        Task<MessageResult> Process(ContactFormMessage message);

    }
}
