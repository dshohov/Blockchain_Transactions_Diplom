using Blockchain_Transactions_Diplom.IHelpers;
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
        private readonly ILiqPayInvoice _liqPay;
        public CoinService(CoinApp coinApp, IServiceProvider serviceProvider, ILiqPayInvoice liqPay)
        {
            _coinApp = coinApp;
            _encryptor = new RSAEncryptor();
            _serviceProvider = serviceProvider;
            _liqPay = liqPay;
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
            if (_coinApp.PerformSuperAdminTransaction(keys, publicKeyUser, amount))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var usersList = await _userManager.GetUsersInRoleAsync("User");
                    var userTo = usersList.FirstOrDefault(x => x.Publickey == publicKeyUser);
                    userTo.Balance += amount;
                    await _userManager.UpdateAsync(userTo);

                }
                return true;
            }
            return false;
        }
        public async Task<bool> SuperAdminCreateTransactionForRecovery(string publicKeyUser, ulong amount)
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
    
        public async Task BuyCoins(string idUser,int countCoins)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                
                var user = await _userManager.FindByIdAsync(idUser);
                var sha256 = new SHA256Hash();
                var orderId = sha256.GetHash(idUser + "#" + countCoins);
                await _liqPay.SendInvoiceAsync(user.Email, countCoins, orderId);
                user.LastOrderId = orderId;
                user.LastCoinBuyCount = (ulong)countCoins;
                await _userManager.UpdateAsync(user);
            }           
        }
        public async Task<bool> CheckInvoiceCoin(string idUser)
        {
            using (var scope = _serviceProvider.CreateScope())
            { 
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var user = await _userManager.FindByIdAsync(idUser);                
                var orderId = user.LastOrderId;
                var checkInvoice = await _liqPay.CheckInvoiceAsync(orderId);

                if (checkInvoice)
                {
                    if(await SuperAdminCreateTransaction(user.Publickey, (ulong)user.LastCoinBuyCount))
                    {
                        user.LastCoinBuyCount = 0;
                        user.LastOrderId = "";
                        await _userManager.UpdateAsync(user);
                        return true;
                    }                   
                }
                return false;
            }
        }
    }
}
