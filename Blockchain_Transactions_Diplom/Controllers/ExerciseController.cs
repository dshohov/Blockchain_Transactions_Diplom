using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Blockchain_Transactions_Diplom.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private readonly IExerciseService _exerciseService;
        public ExerciseController(IExerciseService exerciseService) 
        {
            _exerciseService = exerciseService;        
        }
        [HttpGet]
        public IActionResult CreateExercise()
        {
           
            return View(new ExerciseCreateViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateExerciseAsync([FromForm] ExerciseCreateViewModel exerciseCreateViewModel)
        {
            var idExercise = await _exerciseService.CreateExerciseAsync(exerciseCreateViewModel);
            if (idExercise != "")
                return RedirectToAction("CreateSmartContract", "SmartContract", new { idExercise = idExercise});
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
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
