using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Blockchain_Transactions_Diplom.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [NotNull]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [NotNull]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [NotNull]
        [Compare("Password", ErrorMessage = "Passwords don`t match!")]
        public string? ConfirmPassword { get; set; }
        public string? Code { get; set; }
    }
}
