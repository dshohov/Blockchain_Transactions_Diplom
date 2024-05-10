using Microsoft.AspNetCore.Identity;
using Blockchain_Transactions_Diplom.Interfaces;
using Blockchain_Transactions_Diplom.IServices;
using System.Text.RegularExpressions;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blockchain_Transactions_Diplom.Models;
using System.Security.Claims;

namespace Blockchain_Transactions_Diplom.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPostmarkEmail _sendGridEmail;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICoinService _coinService;
       
        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPostmarkEmail sendGridEmail, RoleManager<IdentityRole> roleManager, ICoinService coinService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sendGridEmail = sendGridEmail;
            _roleManager = roleManager;
            _coinService = coinService;


        }
        
       
        public string GetResetPassword(string currentUrl)
        {            
            string userId="";
            string pattern = @"userId=([^&]+)";
            Match match = Regex.Match(currentUrl, pattern);
            if (match.Success)
            {
                userId = match.Groups[1].Value;

            }
            var emailUser = _userManager.Users.First(c => c.Id == userId).Email;
            
            return emailUser ?? throw new ArgumentNullException(nameof(emailUser));
        }

        

        public async Task<IdentityResult> PostResetPasswordAsync(ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if(user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);
                return result;
            }
            throw new ArgumentNullException();
        }

        public async Task<SignInResult> PostLogin(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.UserEmail);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserEmail, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    if (user != null)
                    {
                        
                        await _userManager.UpdateAsync(user);
                    }
                }
                return result;
                 
            }
            return SignInResult.Failed;
        }

        public async Task PostLogOffAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterViewModel> GetRegisterAsync(string returnUrl)
        {
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
                  
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            
            return registerViewModel;
        }
        public async Task<RegisterViewModel> FailRegister(RegisterViewModel registerViewModel)
        {
            return registerViewModel;
        }

        public async Task<bool> PostRegisterAsync(RegisterViewModel registerViewModel)
        {
            ulong firstBalance = 50;
            var keys = _coinService.GenerateKeyPair();
            var user = new AppUser { Email = registerViewModel.Email, 
                                     FirstName = registerViewModel.FirstName, 
                                     LastName = registerViewModel.LastName, 
                                     UserName = registerViewModel.Email,
                                     Publickey = keys.Publickey,
                                     PrivateKey = keys.PrivateKey,
                                     Balance = firstBalance
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            await _userManager.AddToRoleAsync(user, "User");
            if (result.Succeeded)
            {
                await _coinService.SuperAdminCreateTransaction(user.Publickey, firstBalance);
                return true;
            }
                
            return false;
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email)?? throw new ArgumentNullException();
        }

        public async Task PostForgotPasswordAsync(ForgotPasswordViewModel model,string callbackurl)
        {
            await _sendGridEmail.SendEmailAsync(model.Email, "Reset Email Confirmation", "Please reset email by going to this " +
                    "<a href=\"" + callbackurl + "\">link</a>");
        }

        public async Task<string> CreateCodeAsync(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);

        }
    }
}
