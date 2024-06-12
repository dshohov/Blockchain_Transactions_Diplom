using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface ISmartContractService
    {
        Task<bool> AddExecutorInSmartContractAsync(SmartContractAddExecutor smartContractAddExecutor);
        Task<bool> AddSmartContactAsync(SmartContractCreateViewModel smartContractCreateViewModel);
        Task<IQueryable<SmartContract>> GetFreeSmartContractsAsync();
        Task<SmartContract> GetSmartContractWithExerciseByIdAsync(string contractId);
        Task<bool> AcceptSmartContractAsync(string smartContractId, string userPublicKey);
        Task<IQueryable<SmartContract>> GetMySmartContractsAsync(string creatorPublicKey);
        Task<IQueryable<SmartContract>> GetTasksCompletedByMeAsync(string executorPublicKey);
        Task<bool> ExecutorSendAnsewrAsync(ExerciseExecutorSendAnswerViewModel exerciseExecutorSendAnswerViewModel);
        Task<bool> CreatorSendAnsewrAsync(ExerciseCreatorSendAnswerViewModel exerciseCreatorSendAnswerViewModel);
        Task<bool> PayForWorkAsync(string idExercise);
    }
}
