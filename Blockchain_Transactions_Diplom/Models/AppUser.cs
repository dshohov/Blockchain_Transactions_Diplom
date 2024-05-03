using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Blockchain_Transactions_Diplom.Models
{   
        public class AppUser : IdentityUser
        {
            [NotNull]
            public string? FirstName { get; set; }
            [NotNull]
            public string? LastName { get; set; }
            public string? Publickey { get; set; }
            public string? PrivateKey { get; set; }
            [NotMapped]
            public string? RoleId { get; set; }
            [NotMapped]
            public string? Role { get; set; }
            [NotMapped]
            public IEnumerable<SelectListItem>? RoleList { get; set; }
        }
 
}
