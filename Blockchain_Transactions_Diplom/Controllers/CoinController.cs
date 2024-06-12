using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blockchain_Transactions_Diplom.Controllers
{
    [Authorize]
    public class CoinController : Controller
    {
        private readonly ICoinService _coinService;
        public CoinController(ICoinService coinService)
        {
            _coinService = coinService;
        }
        [HttpGet]
        public IActionResult SoldCoins()
        {            
            return View(new SoldCoinsViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SoldCoinsAsync(SoldCoinsViewModel soldCoinsViewModel)
        {
            if(await _coinService.SoldCoinsAsync(soldCoinsViewModel))
                return RedirectToAction("MessegeAfterSoldCoins");
            return RedirectToAction("Error","Home");
        }
        [HttpGet]
        public IActionResult MessegeAfterSoldCoins()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BuyCoin()
        {            
            return View(new CoinBuyCoinViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> BuyCoin(CoinBuyCoinViewModel coinBuyCoinViewModel)
        {
            await _coinService.BuyCoinsAsync(coinBuyCoinViewModel.UserId,coinBuyCoinViewModel.CountCoin);
            return RedirectToAction("SendInvoice");
        }
        [HttpGet,Authorize]
        public IActionResult SendInvoice()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CheckInvoice(string userId)
        {
            return View(await _coinService.CheckInvoiceCoinAsync(userId));
        }
        [HttpGet]
        public IActionResult CreateTransaction()
        {
            var transactionCreateViewModel = new TransactionCreateViewModel();
            return View(transactionCreateViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTransactionT(TransactionCreateViewModel transactionCreateViewModel)
        {            
            if(await _coinService.CreateTransactionAsync(transactionCreateViewModel))
               return RedirectToAction("Home");
            return RedirectToAction("Error","Home");
        }
        
    }
}
