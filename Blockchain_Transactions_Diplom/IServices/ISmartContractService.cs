using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface ISmartContractService
    {
        Task<bool> AddExecutorInSmartContractAsync(SmartContractAddExecutor smartContractAddExecutor);
        Task<bool> AddSmartContactAsync(SmartContractCreateViewModel smartContractCreateViewModel);
        Task<IQueryable<SmartContract>> GetFreeSmartContracts();
        Task<SmartContract> GetSmartContractWithExerciseById(string contractId);
        Task<bool> AcceptSmartContract(string smartContractId, string userPublicKey);
        Task<IQueryable<SmartContract>> GetMySmartContracts(string creatorPublicKey);
        Task<IQueryable<SmartContract>> GetTasksCompletedByMe(string executorPublicKey);
    }
}
