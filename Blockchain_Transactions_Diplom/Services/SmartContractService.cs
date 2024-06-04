using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using System.Diagnostics.Contracts;
using System.Text;
using XSystem.Security.Cryptography;

namespace Blockchain_Transactions_Diplom.Services
{
    public class SmartContractService : ISmartContractService
    {
        private readonly ISmartContractRepository _smartContractRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ICoinService _coinService;
        public SmartContractService(ISmartContractRepository smartContractRepository, IExerciseRepository exerciseRepository, ICoinService coinService)
        {
            _smartContractRepository = smartContractRepository;
            _exerciseRepository = exerciseRepository;
            _coinService = coinService;
        }

        public async Task<bool> AddSmartContactAsync(SmartContractCreateViewModel smartContractCreateViewModel)
        {
            var idSmartContactComponent = smartContractCreateViewModel.PublicKeyCreator + smartContractCreateViewModel.ContractValue + smartContractCreateViewModel.IdExercise;
            var commision = smartContractCreateViewModel.ContractValue * 0.10;
            var amount = smartContractCreateViewModel.ContractValue - smartContractCreateViewModel.ContractValue * 0.10;

            var smartContract = new SmartContract()
            {
                ContractId = sha256(idSmartContactComponent),
                ContractValue = (ulong)amount,
                PublicKeyCreator = smartContractCreateViewModel.PublicKeyCreator,
                IdExercise = smartContractCreateViewModel.IdExercise,
                IsConfirmed = false
            };
            if (await _coinService.ReturnCoinsToSuperAdmin(smartContractCreateViewModel.UserId,(ulong)commision) && await _coinService.ReturnCoinsToSuperAdmin(smartContractCreateViewModel.UserId, (ulong)amount))
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

        public async Task<SmartContract> GetSmartContractWithExerciseById(string contractId)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(contractId);
            smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            return smartContract;

        }
        public async Task<bool> AcceptSmartContract( string userPublicKey, string smartContractId)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(smartContractId);
            smartContract.PublicKeyExecutor = userPublicKey;
            return await _smartContractRepository.UpdateStateSmartContractAsync(smartContract);
        }

        public async Task<IQueryable<SmartContract>> GetMySmartContracts(string creatorPublicKey)
        {
            var smartContracts = await _smartContractRepository.GetSmartContractsByUserPublicKey(creatorPublicKey);
            foreach(var smartContract in smartContracts.ToList())
            {
                
                smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            }
            return smartContracts;
        }
        public async Task<IQueryable<SmartContract>> GetTasksCompletedByMe(string executorPublicKey)
        {
            var smartContracts = await _smartContractRepository.GetTasksCompletedByMe(executorPublicKey);
            foreach (var smartContract in smartContracts.ToList())
            {

                smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            }
            return smartContracts;
        }
        public async Task<bool> ExecutorSendAnsewrAsync(ExerciseExecutorSendAnswerViewModel exerciseExecutorSendAnswerViewModel)
        {
            var exercise = await _exerciseRepository.GetExerciseAsync(exerciseExecutorSendAnswerViewModel.IdExercise);
            exercise.AnswerExecutor = exerciseExecutorSendAnswerViewModel.AnswerExecutor;
            if (exerciseExecutorSendAnswerViewModel.File != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await exerciseExecutorSendAnswerViewModel.File.CopyToAsync(memoryStream);
                    exercise.FileNameAnswer = exerciseExecutorSendAnswerViewModel.File.FileName;
                    exercise.FileAnswer = memoryStream.ToArray();
                }
            }
            if (await _exerciseRepository.UpdateExerciseAsync(exercise))
                return true;
            return false;
        }

        public async Task<bool> CreatorSendAnsewrAsync(ExerciseCreatorSendAnswerViewModel exerciseCreatorSendAnswerViewModel)
        {
            var exercise = await _exerciseRepository.GetExerciseAsync(exerciseCreatorSendAnswerViewModel.IdExercise);
            exercise.AnswerCreator = exerciseCreatorSendAnswerViewModel.AnwerCreator;           
            if (await _exerciseRepository.UpdateExerciseAsync(exercise))
                return true;
            return false;
        }

        public async Task<bool> PayForWorkAsync(string idExercise)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(idExercise);
            smartContract.IsConfirmed = true;
            
            var commision = smartContract.ContractValue * 0.1;
            var executorGet = smartContract.ContractValue - commision;
            if(await _coinService.SuperAdminCreateTransaction(smartContract.PublicKeyExecutor, (ulong)executorGet))
                return await _smartContractRepository.UpdateStateSmartContractAsync(smartContract);
            return false;

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
