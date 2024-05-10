using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Blockchain_Transactions_Diplom.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult ResetPassword(string? code = null)
        {
            string currentUrl = HttpContext.Request.GetEncodedUrl();
            ViewData["Email"] = _accountService.GetResetPassword(currentUrl);
            return code == null ? RedirectToAction("Error", "Home") : View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.PostResetPasswordAsync(resetPasswordViewModel);               
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    ModelState.AddModelError("Email", "User not found");
                    ViewData["Email"] = resetPasswordViewModel.Email;
                    return View();
                }
            }
            ViewData["Email"] = resetPasswordViewModel.Email;
            return View(resetPasswordViewModel);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.PostLogin(loginViewModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> LogOff()
        {
            await _accountService.PostLogOffAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            var registerViewModel = await _accountService.GetRegisterAsync(returnUrl);
            return View(registerViewModel);
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (registerViewModel != null)
            {
                if(await _accountService.PostRegisterAsync(registerViewModel)) 
                    return LocalRedirect(returnUrl);
                ModelState.AddModelError("Password", "User could not be created. Password not unique enough ");
            }
            return View(await _accountService.FailRegister(registerViewModel));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                var code = await _accountService.CreateCodeAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                if(callbackurl != null)
                {
                    await _accountService.PostForgotPasswordAsync(model, callbackurl);
                    return RedirectToAction("ForgotPasswordConfirmation");
                }                
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAccountInfo()
        {
            return View();
        }

    }
}