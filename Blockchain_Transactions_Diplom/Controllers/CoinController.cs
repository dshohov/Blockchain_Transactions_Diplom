using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;

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
