using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Microsoft.AspNetCore.Identity;

namespace Blockchain_Transactions_Diplom.Initializer
{
    public class RecoveryInitializer
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICoinService _coinService;

        public RecoveryInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ICoinService coinService)
        {
            _userManager = userManager;
            _roleManager = roleManager;            
            _coinService = coinService;
        }

        public async Task InitializeAsync()
        {
            await RecoveryBlockchainAsync();
        }
        private async Task RecoveryBlockchainAsync()
        {
            // Проверяем наличие роли User
            if (await _roleManager.RoleExistsAsync("User"))
            {
                // Проверяем наличие пользователей
                var usersList = await _userManager.GetUsersInRoleAsync("User");
                if(usersList.Count > 0)
                {
                    var superAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
                    if (superAdmins.Count == 1)
                    {                        
                        foreach (var user in usersList)
                        {
                            if(user.Balance > 0)
                            {
                                if(user.Publickey != null)
                                    await _coinService.SuperAdminCreateTransactionForRecoveryAsync(user.Publickey, (ulong)user.Balance);
                            }
                        }
                    }
                    
                }
            }
        }

    }
}
