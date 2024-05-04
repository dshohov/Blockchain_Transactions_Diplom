using Microsoft.AspNetCore.Identity;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using System.Security.Claims;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface IAccountService
    {
        string GetResetPassword(string currentUrl);
        Task<IdentityResult> PostResetPasswordAsync(ResetPasswordViewModel resetPasswordViewModel);
        Task<SignInResult> PostLogin(LoginViewModel loginViewModel);
        Task PostLogOffAsync();
        Task<RegisterViewModel> FailRegister(RegisterViewModel registerViewModel);
        Task<RegisterViewModel> GetRegisterAsync(string returnUrl);
        Task<bool> PostRegisterAsync(RegisterViewModel registerViewModel);
        Task PostForgotPasswordAsync(ForgotPasswordViewModel model, string callbackurl);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<string> CreateCodeAsync(AppUser user);
        Task<AccountInfoViewModel> GetUserInfoAsync(ClaimsPrincipal user);
    }
}
