using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using System.Text;


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
                ContractValue = amount != null ? (ulong)amount : 0,
                PublicKeyCreator = smartContractCreateViewModel.PublicKeyCreator,
                IdExercise = smartContractCreateViewModel.IdExercise,
                IsConfirmed = false
            };
            if(smartContractCreateViewModel.UserId != null && commision != null && amount != null)
                if (await _coinService.ReturnCoinsToSuperAdminAsync(smartContractCreateViewModel.UserId,(ulong)commision) && await _coinService.ReturnCoinsToSuperAdminAsync(smartContractCreateViewModel.UserId, (ulong)amount))
                    if (await _smartContractRepository.CreateSmartContractAsync(smartContract))
                        return true;
            return false;


        }

        public async Task<bool> AddExecutorInSmartContractAsync(SmartContractAddExecutor smartContractAddExecutor)
        {
            if(smartContractAddExecutor.ContractId != null)
            {
                var smartContract = await _smartContractRepository.GetSmartContractAsync(smartContractAddExecutor.ContractId);
                smartContract.PublicKeyExecutor = smartContractAddExecutor.PublicKeyExecutor;
                if (await _smartContractRepository.UpdateStateSmartContractAsync(smartContract))
                    return true;
            }            
            return false;
        }
        
        public async Task<IQueryable<SmartContract>> GetFreeSmartContractsAsync()
        {
            var smartContracts = await _smartContractRepository.GetFreeSmartContractsAsync();
            foreach(var smartContract in smartContracts.ToList())
            {
                if(smartContract.IdExercise != null)
                {
                    smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
                }
            }
            return smartContracts;
        }

        public async Task<SmartContract> GetSmartContractWithExerciseByIdAsync(string contractId)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(contractId);
            if(smartContract.IdExercise != null)
                smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            
            return smartContract;


        }
        public async Task<bool> AcceptSmartContractAsync( string userPublicKey, string smartContractId)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(smartContractId);
            smartContract.PublicKeyExecutor = userPublicKey;
            return await _smartContractRepository.UpdateStateSmartContractAsync(smartContract);
        }

        public async Task<IQueryable<SmartContract>> GetMySmartContractsAsync(string creatorPublicKey)
        {
            var smartContracts = await _smartContractRepository.GetSmartContractsByUserPublicKeyAsync(creatorPublicKey);
            foreach(var smartContract in smartContracts.ToList())
            {
                if(smartContract.IdExercise != null)
                    smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            }
            return smartContracts;
        }
        public async Task<IQueryable<SmartContract>> GetTasksCompletedByMeAsync(string executorPublicKey)
        {
            var smartContracts = await _smartContractRepository.GetTasksCompletedByMeAsync(executorPublicKey);
            foreach (var smartContract in smartContracts.ToList())
            {
                if(smartContract.IdExercise != null)
                    smartContract.Exercise = await _exerciseRepository.GetExerciseAsync(smartContract.IdExercise);
            }
            return smartContracts;
        }
        public async Task<bool> ExecutorSendAnsewrAsync(ExerciseExecutorSendAnswerViewModel exerciseExecutorSendAnswerViewModel)
        {
            if(exerciseExecutorSendAnswerViewModel.IdExercise != null)
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
            }            
            return false;
        }

        public async Task<bool> CreatorSendAnsewrAsync(ExerciseCreatorSendAnswerViewModel exerciseCreatorSendAnswerViewModel)
        {
            if(exerciseCreatorSendAnswerViewModel.IdExercise != null)
            {
                var exercise = await _exerciseRepository.GetExerciseAsync(exerciseCreatorSendAnswerViewModel.IdExercise);
                exercise.AnswerCreator = exerciseCreatorSendAnswerViewModel.AnwerCreator;
                if (await _exerciseRepository.UpdateExerciseAsync(exercise))
                    return true;
            }
            
            return false;
        }

        public async Task<bool> PayForWorkAsync(string idExercise)
        {
            var smartContract = await _smartContractRepository.GetSmartContractAsync(idExercise);
            smartContract.IsConfirmed = true;
            
            var commision = smartContract.ContractValue * 0.1;
            var executorGet = smartContract.ContractValue - commision;
            if(smartContract.PublicKeyExecutor != null && executorGet != null)
                if(await _coinService.SuperAdminCreateTransactionAsync(smartContract.PublicKeyExecutor, (ulong)executorGet))
                    return await _smartContractRepository.UpdateStateSmartContractAsync(smartContract);
            return false;

        }


        private static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.HMACSHA256();
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
