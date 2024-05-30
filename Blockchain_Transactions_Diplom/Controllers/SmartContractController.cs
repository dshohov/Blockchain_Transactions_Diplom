using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blockchain_Transactions_Diplom.Controllers
{
    public class SmartContractController : Controller
    {
        private readonly ISmartContractService _smartContractService;
        public SmartContractController(ISmartContractService smartContractService)
        {
            _smartContractService = smartContractService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
            return View(await _smartContractService.GetFreeSmartContracts());
        }

        
        public async Task<IActionResult> Details(string idSmartContract)
        {
            return View(await _smartContractService.GetSmartContractWithExerciseById(idSmartContract));
        }

        public async Task<IActionResult> AcceptSmartContract(string userPublicKey, string idSmartContract)
        {
            await _smartContractService.AcceptSmartContract(userPublicKey, idSmartContract);
            return View();
        }

        public async Task<IActionResult> MySmartContracts(string creatorPublicKey)
        {
            return View(await _smartContractService.GetMySmartContracts(creatorPublicKey));
        }
        public async Task<IActionResult> TasksCompletedByMe(string executorPublicKey)
        {
            return View(await _smartContractService.GetTasksCompletedByMe(executorPublicKey));
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
    }
}
