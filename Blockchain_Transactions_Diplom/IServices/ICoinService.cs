using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface ICoinService
    {
        public KeyPair GenerateKeyPair();
        public Task<bool> CreateTransactionAsync(TransactionCreateViewModel transactionCreateViewModel);
        public Task<bool> SuperAdminCreateTransactionAsync(string publicKeyUser, ulong amount);
        public Task<bool> SuperAdminCreateTransactionForRecoveryAsync(string publicKeyUser, ulong amount);
        public Task BuyCoinsAsync(string idUser, int countCoins);
        public Task<bool> CheckInvoiceCoinAsync(string idUser);
        public Task<bool> SoldCoinsAsync(SoldCoinsViewModel soldCoinsViewModel);
        public Task<bool> ReturnCoinsToSuperAdminAsync(string userId, ulong amounCoins);
    }
}
