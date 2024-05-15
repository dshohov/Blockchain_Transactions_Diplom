using Blockchain_Transactions_Diplom.Models;

namespace Blockchain_Transactions_Diplom.IRepositories
{
    public interface ISmartContractRepository
    {
        public Task<bool> CreateSmartContractAsync(SmartContract smartContract);
        public Task<SmartContract> GetSmartContractAsync(string idSmartContract);
        public Task<bool> DeleteSmartContractAsync(string idSmartContract);
    }
}
