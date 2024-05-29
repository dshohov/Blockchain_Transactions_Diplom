using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface ISmartContractService
    {
        Task<bool> AddExecutorInSmartContractAsync(SmartContractAddExecutor smartContractAddExecutor);
        Task<bool> AddSmartContactAsync(SmartContractCreateViewModel smartContractCreateViewModel);
        Task<IQueryable<SmartContract>> GetFreeSmartContracts();
        Task<SmartContract> GetSmartContractById(string contractId);
    }
}
