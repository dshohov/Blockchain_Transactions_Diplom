using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Postmark.Model.Suppressions;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Blockchain_Transactions_Diplom.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExerciseService(IExerciseRepository exerciseRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _exerciseRepository = exerciseRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CreateExerciseAsync(ExerciseCreateViewModel exerciseCreateViewModel)
        {
            var exercise = new Exercise()
            {
                Name = exerciseCreateViewModel.Name,
                Description = exerciseCreateViewModel.Description,                
                UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };
            if (exerciseCreateViewModel.File != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await exerciseCreateViewModel.File.CopyToAsync(memoryStream);
                    exercise.FileName = exerciseCreateViewModel.File.FileName;
                    exercise.File = memoryStream.ToArray();
                }
            }
            return await _exerciseRepository.CreateExerciseAsync(exercise);
        }
        public async Task<Exercise> GetExerciseByIdAsync(int idExercise)
        { 
            return await _exerciseRepository.GetExerciseAsync(idExercise);
        }
    }
}
