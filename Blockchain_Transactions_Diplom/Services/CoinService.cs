using Blockchain_Transactions_Diplom.IHelpers;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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
        private async Task<KeyPair?> GetUserKeyPairSuperAdminAsync()
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

        public async Task<bool> CreateTransactionAsync(TransactionCreateViewModel transactionCreateViewModel)
        {
            var fromKeys = new KeyPair(transactionCreateViewModel.FromPublicKey, transactionCreateViewModel.FromPrivateKey);
            var comision = transactionCreateViewModel.Amount * 0.05;
            if (comision == transactionCreateViewModel.Amount)
                comision = 1;
            transactionCreateViewModel.Amount -= (ulong)comision;
            if (await Task.Run(() => _coinApp.PerformTransaction(fromKeys, transactionCreateViewModel.ToPublicKey, transactionCreateViewModel.Amount)))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var superAdminKeys = await GetUserKeyPairSuperAdminAsync();
                    if(superAdminKeys != null)
                    {
                        await Task.Run(() => _coinApp.PerformTransaction(fromKeys, superAdminKeys.Publickey, (ulong)comision));

                        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                        var usersList = await _userManager.GetUsersInRoleAsync("User");
                        var userFrom = usersList.FirstOrDefault(x => x.Publickey == transactionCreateViewModel.FromPublicKey);
                        var userTo = usersList.FirstOrDefault(x => x.Publickey == transactionCreateViewModel.ToPublicKey);
                        if (userFrom != null && userTo != null)
                        {
                            userFrom.Balance -= transactionCreateViewModel.Amount;
                            userTo.Balance += transactionCreateViewModel.Amount;
                            await _userManager.UpdateAsync(userFrom);
                            await _userManager.UpdateAsync(userTo);
                        }

                    }
                }
                return true;
            }
               
            return false;
        }
        public async Task<bool> SoldCoinTransactionAsync(TransactionCreateViewModel transactionCreateViewModel)
        {
            var fromKeys = new KeyPair(transactionCreateViewModel.FromPublicKey, transactionCreateViewModel.FromPrivateKey);
            if (await Task.Run(() => _coinApp.PerformTransaction(fromKeys, transactionCreateViewModel.ToPublicKey, transactionCreateViewModel.Amount)))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var usersList = await _userManager.GetUsersInRoleAsync("User");
                    var userFrom = usersList.FirstOrDefault(x => x.Publickey == transactionCreateViewModel.FromPublicKey);     
                    if(userFrom != null)
                    {
                        userFrom.Balance -= transactionCreateViewModel.Amount;
                        await _userManager.UpdateAsync(userFrom);
                    }
                    
                }
                return true;
            }

            return false;
        }
        public async Task<bool> SuperAdminCreateTransactionAsync(string publicKeyUser, ulong amount)
        {
            var keys = await GetUserKeyPairSuperAdminAsync();
            if (_coinApp.PerformSuperAdminTransaction(keys, publicKeyUser, amount))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var usersList = await _userManager.GetUsersInRoleAsync("User");
                    var userTo = usersList.FirstOrDefault(x => x.Publickey == publicKeyUser);
                    if(userTo != null)
                    {
                        userTo.Balance += amount;
                        await _userManager.UpdateAsync(userTo);
                    }
                   

                }
                return true;
            }
            return false;
        }
        public async Task<bool> SuperAdminCreateTransactionForRecoveryAsync(string publicKeyUser, ulong amount)
        {
            var keys = await GetUserKeyPairSuperAdminAsync();
            if (await Task.Run(() => _coinApp.PerformSuperAdminTransaction(keys, publicKeyUser, amount)))
            {                
                return true;
            }
            return false;
        }

    
        public async Task BuyCoinsAsync(string idUser,int countCoins)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                
                var user = await _userManager.FindByIdAsync(idUser);
                var sha256 = new SHA256Hash();
                var orderId = sha256.GetHash(idUser + "#" + countCoins);
                if(user != null)
                {
                    await _liqPay.SendInvoiceAsync(user.Email, countCoins, orderId);
                    user.LastOrderId = orderId;
                    user.LastCoinBuyCount = (ulong)countCoins;
                    await _userManager.UpdateAsync(user);
                }
                
            }           
        }
        public async Task<bool> CheckInvoiceCoinAsync(string idUser)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var user = await _userManager.FindByIdAsync(idUser);
                if (user != null)
                {
                    var orderId = user.LastOrderId;
                    if (orderId != null)
                    {
                        var checkInvoice = await _liqPay.CheckInvoiceAsync(orderId);

                        if (checkInvoice)
                        {
                            if(user.Publickey != null && user.LastCoinBuyCount != null)
                            {
                                if (await SuperAdminCreateTransactionAsync(user.Publickey, (ulong)user.LastCoinBuyCount))
                                {
                                    user.LastCoinBuyCount = 0;
                                    user.LastOrderId = "";
                                    await _userManager.UpdateAsync(user);
                                    return true;
                                }
                            }                            
                        }
                    }
                    
                }
               
                return false;
            }
        }

        public async Task<bool> SoldCoinsAsync(SoldCoinsViewModel soldCoinsViewModel)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if(soldCoinsViewModel.UserId != null)
                {
                    var user = await _userManager.FindByIdAsync(soldCoinsViewModel.UserId);
                    if (user != null) {
                        if (user.Balance >= soldCoinsViewModel.CountCoins)
                        {
                            var superAdminKeys = await GetUserKeyPairSuperAdminAsync();
                            var transaction = new TransactionCreateViewModel()
                            {
                                FromPublicKey = user.Publickey,
                                FromPrivateKey = user.PrivateKey,
                                ToPublicKey = superAdminKeys.Publickey,
                                Amount = (ulong)soldCoinsViewModel.CountCoins
                            };
                            if (await SoldCoinTransactionAsync(transaction))
                            {
                                return true;
                            }
                        }
                    }
                    
                }
                
            }
            return false;
        }

        public async Task<bool> ReturnCoinsToSuperAdminAsync(string userId,ulong amounCoins)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    if (user.Balance >= amounCoins)
                    {
                        var superAdminKeys = await GetUserKeyPairSuperAdminAsync();
                        var transaction = new TransactionCreateViewModel()
                        {
                            FromPublicKey = user.Publickey,
                            FromPrivateKey = user.PrivateKey,
                            ToPublicKey = superAdminKeys.Publickey,
                            Amount = (ulong)amounCoins
                        };
                        if (await SoldCoinTransactionAsync(transaction))
                        {
                            return true;
                        }
                    }
                }
               
            }
            return false;
        }
    }
}
