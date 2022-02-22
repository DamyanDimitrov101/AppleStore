using System.Threading.Tasks;
using AppleStore.ViewModels;

namespace AppleStore.Services.Contracts
{
    public interface IAccountService
    {
        Task<IndexViewModel> GetAccountViewModel(string userId);
    }
}
