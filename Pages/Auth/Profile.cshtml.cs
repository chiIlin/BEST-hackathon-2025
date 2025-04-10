using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BEST_hackathon_2025.Pages.Auth
{
    public class ProfileModel : PageModel
    {
        public string UserInfo { get; private set; }

        public void OnGet()
        {
            UserInfo = "Це тестова інформація про користувача.";
        }
    }
}
