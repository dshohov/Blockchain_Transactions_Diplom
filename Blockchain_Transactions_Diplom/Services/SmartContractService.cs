﻿using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using System.Text;
using XSystem.Security.Cryptography;

namespace Blockchain_Transactions_Diplom.Services
{
    public class SmartContractService : ISmartContractService
    {
        private readonly ISmartContractRepository _smartContractRepository;
        private readonly IExerciseRepository _exerciseRepository;
        public SmartContractService(ISmartContractRepository smartContractRepository, IExerciseRepository exerciseRepository)
        {
            _smartContractRepository = smartContractRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<bool> AddSmartContactAsync(SmartContractCreateViewModel smartContractCreateViewModel)
        {
            var idSmartContactComponent = smartContractCreateViewModel.PublicKeyCreator + smartContractCreateViewModel.ContractValue + smartContractCreateViewModel.IdExercise;
           
            var smartContract = new SmartContract()
            {
                ContractId = sha256(idSmartContactComponent),
                ContractValue = smartContractCreateViewModel.ContractValue,
                PublicKeyCreator = smartContractCreateViewModel.PublicKeyCreator,
                IdExercise = smartContractCreateViewModel.IdExercise,
                IsConfirmed = false
            };

            if (await _smartContractRepository.CreateSmartContractAsync(smartContract))
                return true;
            return false;

        }

        public async Task<bool> AddExecutorInSmartContractAsync(SmartContractAddExecutor smartContractAddExecutor)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(smartContractAddExecutor.ContractId);
            smartContract.PublicKeyExecutor = smartContractAddExecutor.PublicKeyExecutor;
            if (await _smartContractRepository.UpdateStateSmartContractAsync(smartContract))
                return true;
            return false;
        }
        
        public async Task<IQueryable<SmartContract>> GetFreeSmartContracts()
        {
            var smartContracts = await _smartContractRepository.GetFreeSmartContracts();
            foreach(var smartContract in smartContracts.ToList())
            {
                smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            }
            return smartContracts;
        }

        public async Task<SmartContract> GetSmartContractById(string contractId)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(contractId);
            smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            return smartContract;

        }









        private static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
