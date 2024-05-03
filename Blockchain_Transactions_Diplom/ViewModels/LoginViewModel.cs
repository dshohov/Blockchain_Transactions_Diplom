using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Blockchain_Transactions_Diplom.ViewModels
{
    public class LoginViewModel
    {
        [NotNull]
        [Required]
        public string? UserEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [NotNull]
        public string? Password { get; set; }
        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
