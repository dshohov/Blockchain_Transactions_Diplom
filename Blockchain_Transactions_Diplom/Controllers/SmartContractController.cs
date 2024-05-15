using Microsoft.AspNetCore.Mvc;

namespace Blockchain_Transactions_Diplom.Controllers
{
    public class SmartContractController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
