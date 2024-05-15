using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blockchain_Transactions_Diplom.Controllers
{
    public class CoinController : Controller
    {
        private readonly ICoinService _coinService;
        public CoinController(ICoinService coinService)
        {
            _coinService = coinService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyCoin()
        {
            await _coinService.BuyCoins("3da9bd94-ef03-4035-9b87-6ae0c3203fd1",50);
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateTransaction()
        {
            var transactionCreateViewModel = new TransactionCreateViewModel();
            return View(transactionCreateViewModel);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransactionT(TransactionCreateViewModel transactionCreateViewModel)
        {            
            if(await _coinService.CreateTransaction(transactionCreateViewModel))
               return RedirectToAction("Home");
            return RedirectToAction("Error");
        }
        
    }
}
