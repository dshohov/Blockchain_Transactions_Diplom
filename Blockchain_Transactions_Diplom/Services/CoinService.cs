using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;

namespace Blockchain_Transactions_Diplom.Services
{
    public class CoinService : ICoinService
    {

        private CoinApp _coinApp;
        private readonly RSAEncryptor _encryptor;
        private readonly IServiceProvider _serviceProvider;

        public CoinService(CoinApp coinApp, IServiceProvider serviceProvider)
        {
            _coinApp = coinApp;
            _encryptor = new RSAEncryptor();
            _serviceProvider = serviceProvider;
        }
        public KeyPair GenerateKeyPair() => _encryptor.GenerateKeys();
        private async Task<KeyPair> GetUserKeyPairSuperAdminAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var superAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
                var superAdmin = superAdmins.FirstOrDefault();
                if (superAdmin != null)
                {
                    return new KeyPair(superAdmin.Publickey, superAdmin.PrivateKey);
                }
            }
            return null;
        }

        public async Task<bool> CreateTransaction(TransactionCreateViewModel transactionCreateViewModel)
        {
            var fromKeys = new KeyPair(transactionCreateViewModel.FromPublicKey, transactionCreateViewModel.FromPrivateKey);
            if (await Task.Run(() => _coinApp.PerformTransaction(fromKeys, transactionCreateViewModel.ToPublicKey, transactionCreateViewModel.Amount)))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var usersList = await _userManager.GetUsersInRoleAsync("User");
                    var userFrom =  usersList.FirstOrDefault(x=>x.Publickey == transactionCreateViewModel.FromPublicKey);
                    var userTo = usersList.FirstOrDefault(x => x.Publickey == transactionCreateViewModel.ToPublicKey);
                    userFrom.Balance -= transactionCreateViewModel.Amount;
                    userTo.Balance += transactionCreateViewModel.Amount;
                    await _userManager.UpdateAsync(userFrom);
                    await _userManager.UpdateAsync(userTo);

                }
                return true;
            }
               
            return false;
        }
        public async Task<bool> SuperAdminCreateTransaction(string publicKeyUser, ulong amount)
        {
            var keys = await GetUserKeyPairSuperAdminAsync();
            if (await Task.Run(() => _coinApp.PerformSuperAdminTransaction(keys, publicKeyUser, amount)))
            {
                return true;
            }
            return false;
        }

        public async Task<ulong> BalanceСheck(KeyPair keyPair)
        {
            var balance = await Task.Run(() => _coinApp.GetBalanceUser(keyPair));
            return balance;

        }
    }
}
