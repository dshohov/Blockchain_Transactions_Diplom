﻿using Blockchain_Transactions_Diplom.Data;
using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.Models;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Transactions_Diplom.Repositories
{
    public class SmartContractRepository : ISmartContractRepository
    {
        private readonly ApplicationDBContext _context;

        public SmartContractRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateSmartContractAsync(SmartContract smartContract)
        {
            await _context.AddAsync(smartContract);
            return await SaveAsync();
        }
        public async Task<SmartContract> GetSmartContractAsync(string idSmartContract)
        {
            var smartContract = await _context.SmartContracts.FirstOrDefaultAsync(x => x.ContractId == idSmartContract);
            if (smartContract == null)
                return new SmartContract();
            return smartContract;
        }
        public async Task<bool> DeleteSmartContractAsync(string idSmartContract)
        {
            var exercise = await GetSmartContractAsync(idSmartContract);
            await Task.Run(() => _context.Remove(exercise));
            return await SaveAsync();
        }
        public async Task<bool> UpdateStateSmartContractAsync(SmartContract smartContract)
        {
            await Task.Run(() => _context.Update(smartContract));
            return await SaveAsync();
        }

        public async Task<IQueryable<SmartContract>> GetFreeSmartContractsAsync() => await Task.Run(()=> _context.SmartContracts.Where(x=>x.PublicKeyExecutor == null));

        private async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<IQueryable<SmartContract>> GetSmartContractsByUserPublicKeyAsync(string creatorPublicKey)
        {
            return await Task.Run(()=> _context.SmartContracts.Where(x=>x.PublicKeyCreator == creatorPublicKey));
        }
        
        public async Task<IQueryable<SmartContract>> GetTasksCompletedByMeAsync(string executorPublicKey)
        {
            return await Task.Run(() => _context.SmartContracts.Where(x => x.PublicKeyExecutor == executorPublicKey && x.IsConfirmed == false));
        }
    }
}
