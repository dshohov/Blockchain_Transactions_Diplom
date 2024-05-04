using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Blockchain_Transactions_Diplom.ViewModels
{
    public class AccountInfoViewModel
    {

        [NotNull]
        public string? FirstName { get; set; }
        [NotNull]
        public string? LastName { get; set; }
        public string? Publickey { get; set; }
        public string? PrivateKey { get; set; }       
    }
}
