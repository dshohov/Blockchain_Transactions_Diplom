using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface ICoinService
    {
        public KeyPair GenerateKeyPair();
        public Task<bool> CreateTransaction(TransactionCreateViewModel transactionCreateViewModel);
        public Task<bool> SuperAdminCreateTransaction(string publicKeyUser, ulong amount);
        public Task<bool> SuperAdminCreateTransactionForRecovery(string publicKeyUser, ulong amount);
        public Task<ulong> BalanceСheck(KeyPair keyPair);
        public Task BuyCoins(string idUser, int countCoins);
        public Task<bool> CheckInvoiceCoin(string idUser);
    }
}
