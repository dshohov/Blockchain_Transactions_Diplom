using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Blockchain_Transactions_Diplom.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [NotNull]
        public string? Email { get; set; }
    }
}
