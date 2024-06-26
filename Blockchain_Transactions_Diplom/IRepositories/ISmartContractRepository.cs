﻿using Blockchain_Transactions_Diplom.Models;

namespace Blockchain_Transactions_Diplom.IRepositories
{
    public interface ISmartContractRepository
    {
        Task<bool> UpdateStateSmartContractAsync(SmartContract smartContract);
        Task<bool> CreateSmartContractAsync(SmartContract smartContract);
        Task<SmartContract> GetSmartContractAsync(string idSmartContract);
        Task<bool> DeleteSmartContractAsync(string idSmartContract);
        Task<IQueryable<SmartContract>> GetFreeSmartContractsAsync();

        Task<IQueryable<SmartContract>> GetSmartContractsByUserPublicKeyAsync(string creatorPublicKey);
        Task<IQueryable<SmartContract>> GetTasksCompletedByMeAsync(string executorPublicKey);
    }
}
