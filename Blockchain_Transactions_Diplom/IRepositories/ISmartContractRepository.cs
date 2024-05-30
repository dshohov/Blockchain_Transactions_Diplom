using Blockchain_Transactions_Diplom.Models;

namespace Blockchain_Transactions_Diplom.IRepositories
{
    public interface ISmartContractRepository
    {
        Task<bool> UpdateStateSmartContractAsync(SmartContract smartContract);
        Task<bool> CreateSmartContractAsync(SmartContract smartContract);
        Task<SmartContract> GetSmartContractAsync(string idSmartContract);
        Task<bool> DeleteSmartContractAsync(string idSmartContract);
        Task<IQueryable<SmartContract>> GetFreeSmartContracts();

        Task<IQueryable<SmartContract>> GetSmartContractsByUserPublicKey(string creatorPublicKey);
        Task<IQueryable<SmartContract>> GetTasksCompletedByMe(string executorPublicKey);
    }
}
