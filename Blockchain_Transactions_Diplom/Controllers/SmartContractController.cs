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

    }
}
