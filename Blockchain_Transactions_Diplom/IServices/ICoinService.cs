using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface ICoinService
    {
        public KeyPair GenerateKeyPair();
        public bool CreateTransaction(TransactionCreateViewModel transactionCreateViewModel);
        public Task<bool> SuperAdminCreateTransaction(string publicKeyUser, ulong amount);
    }
}
