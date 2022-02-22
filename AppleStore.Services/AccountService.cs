using System.Threading.Tasks;
using AppleStore.Services.Contracts;
using AppleStore.ViewModels;
using Microsoft.AspNet.Identity;

namespace AppleStore.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationUserManager userManager;
        public AccountService(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IndexViewModel> GetAccountViewModel(string userId)
        {
            return new IndexViewModel
            {
                HasPassword = HasPassword(userId),
                PhoneNumber = await this.userManager.GetPhoneNumberAsync(userId),
                TwoFactor = await this.userManager.GetTwoFactorEnabledAsync(userId),
                Logins = await this.userManager.GetLoginsAsync(userId),
                BrowserRemembered = false
            };
        }

        private bool HasPassword(string userId)
        {
            var user = this.userManager.FindById(userId);
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
    }
}
