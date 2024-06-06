using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blockchain_Transactions_Diplom.Controllers
{
    [Authorize]
    public class SmartContractController : Controller
    {
        private readonly ISmartContractService _smartContractService;
        public SmartContractController(ISmartContractService smartContractService)
        {
            _smartContractService = smartContractService;
        }

        [HttpGet]
        public IActionResult CreateSmartContract(string idExercise)
        {
            return View(new SmartContractCreateViewModel() { IdExercise = idExercise });
        }
        [HttpPost]
        public async Task<IActionResult> CreateSmartContractAsync(SmartContractCreateViewModel smartContractCreateViewModel)
        {
            if (await _smartContractService.AddSmartContactAsync(smartContractCreateViewModel))
                return RedirectToAction("Index");
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> GetFreeSmartContracts()
        {
            return View(await _smartContractService.GetFreeSmartContractsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(string idSmartContract)
        {
            return View(await _smartContractService.GetSmartContractWithExerciseByIdAsync(idSmartContract));
        }
        [HttpGet]
        public async Task<IActionResult> AcceptSmartContract(string userPublicKey, string idSmartContract)
        {
            if (await _smartContractService.AcceptSmartContractAsync(userPublicKey, idSmartContract))
                return View();
            return RedirectToAction("Error","Home");
        }
        [HttpGet]
        public async Task<IActionResult> MySmartContracts(string creatorPublicKey)
        {
            return View(await _smartContractService.GetMySmartContractsAsync(creatorPublicKey));
        }
        [HttpGet]
        public async Task<IActionResult> TasksCompletedByMe(string executorPublicKey)
        {
            return View(await _smartContractService.GetTasksCompletedByMeAsync(executorPublicKey));
        }
        [HttpGet]
        public IActionResult ExecutorSendAnsewr(string idExercise)
        {
            return View(new ExerciseExecutorSendAnswerViewModel() { IdExercise = idExercise});
        }
        [HttpPost]
        public async Task<IActionResult> ExecutorSendAnsewrAsync(ExerciseExecutorSendAnswerViewModel exerciseExecutorSendAnswerViewModel)
        {
            if(await _smartContractService.ExecutorSendAnsewrAsync(exerciseExecutorSendAnswerViewModel))
                return RedirectToAction("GetFreeSmartContracts");
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public IActionResult SendReplyToExecutor(string idExericse)
        {
            return View(new ExerciseCreatorSendAnswerViewModel() { IdExercise = idExericse });
        }
        [HttpPost]
        public async Task<IActionResult> SendReplyToExecutor(ExerciseCreatorSendAnswerViewModel exerciseCreatorSendAnswerViewModel)
        {
            if (await _smartContractService.CreatorSendAnsewrAsync(exerciseCreatorSendAnswerViewModel))
                return RedirectToAction("GetFreeSmartContracts");
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> PayForWork(string idSmartContract)
        {
            if (await _smartContractService.PayForWorkAsync(idSmartContract))
                return RedirectToAction("GetFreeSmartContracts");
            return RedirectToAction("Error", "Home");
        }
    }
}
