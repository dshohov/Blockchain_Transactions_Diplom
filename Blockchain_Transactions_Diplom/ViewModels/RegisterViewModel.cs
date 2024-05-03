using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Blockchain_Transactions_Diplom.ViewModels
{
    public class RegisterViewModel
    {
        [EmailAddress]
        [NotNull]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [NotNull]
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; }
        [Required]
        [NotNull]
        [Display(Name = "LastName")]
        public string? LastName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [NotNull]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [NotNull]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
