using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.ViewModels;
using LiqPay.SDK.Dto.Enums;
using LiqPay.SDK.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using LiqPay.SDK;

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
            return View(new CoinBuyCoinViewModel());
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BuyCoin(CoinBuyCoinViewModel coinBuyCoinViewModel)
        {
            await _coinService.BuyCoins(coinBuyCoinViewModel.UserId,coinBuyCoinViewModel.CountCoin);
            return RedirectToAction("SendInvoice");
        }
        [HttpGet,Authorize]
        public IActionResult SendInvoice()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckInvoice(string userId)
        {
            return View(await _coinService.CheckInvoiceCoin(userId));
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
