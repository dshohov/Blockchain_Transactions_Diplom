using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface ICoinService
    {
        public KeyPair GenerateKeyPair();
        public Task<bool> CreateTransaction(TransactionCreateViewModel transactionCreateViewModel);
        public Task<bool> SuperAdminCreateTransaction(string publicKeyUser, ulong amount);
        public Task<ulong> BalanceСheck(KeyPair keyPair);
    }
}
