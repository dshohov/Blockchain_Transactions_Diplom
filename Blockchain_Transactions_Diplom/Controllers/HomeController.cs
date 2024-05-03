using Blockchain_Transactions_Diplom.Models;
using Microsoft.AspNetCore.Mvc;
using PostmarkDotNet.Model;
using PostmarkDotNet;
using System.Diagnostics;
using PostmarkDotNet;

namespace Blockchain_Transactions_Diplom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //var message = new PostmarkMessage()
            //{
            //    To = "d.shokhov@student.csn.khai.edu",
            //    From = "d.shokhov@student.csn.khai.edu",
            //    TrackOpens = true,
            //    Subject = "A complex email",
            //    TextBody = "Plain Text Body",
            //    HtmlBody = "HTML goes here",
            //    Tag = "New Year's Email Campaign",
            //    Headers = new HeaderCollection{
                   
            //      }
            //};

            //var client = new PostmarkClient("d1a3fcf9-9135-437d-80c6-23b2bb1c1740");
            //var sendResult = await client.SendMessageAsync(message);

            //if (sendResult.Status == PostmarkStatus.Success) { /* Handle success */ }
            //else { /* Resolve issue.*/ }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}