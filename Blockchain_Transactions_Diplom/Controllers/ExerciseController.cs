using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Blockchain_Transactions_Diplom.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly IExerciseService _exerciseService;
        private readonly UserManager<AppUser> _userManager;
        public ExerciseController(IExerciseService exerciseService, UserManager<AppUser> userManager) 
        {
            _exerciseService = exerciseService;        
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult CreateExercise()
        {
           
            return View(new ExerciseCreateViewModel());
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateExerciseAsync([FromForm] ExerciseCreateViewModel exerciseCreateViewModel)
        {
            var idExercise = await _exerciseService.CreateExerciseAsync(exerciseCreateViewModel);
            if (idExercise != "")
                return RedirectToAction("CreateSmartContract", "SmartContract", new { idExercise = idExercise});
            return RedirectToAction("Error");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);

            if (exercise == null || exercise.File == null)
            {
                return NotFound();
            }
            string mimeType = MimeTypes.GetMimeType(exercise.FileName);
            var memoryStream = new MemoryStream(exercise.File);
            return File(memoryStream, mimeType, exercise.FileName);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DownloadFileAnswer(string id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);

            if (exercise == null || exercise.FileAnswer == null)
            {
                return NotFound();
            }
            string mimeType = MimeTypes.GetMimeType(exercise.FileNameAnswer);
            var memoryStream = new MemoryStream(exercise.FileAnswer);
            return File(memoryStream, mimeType, exercise.FileNameAnswer);
        }
        
    }
}
