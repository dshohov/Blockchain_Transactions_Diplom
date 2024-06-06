using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace Blockchain_Transactions_Diplom.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExerciseService(IExerciseRepository exerciseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _exerciseRepository = exerciseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreateExerciseAsync(ExerciseCreateViewModel exerciseCreateViewModel)
        {
            var exercise = new Exercise()
            {
                Id = sha256(exerciseCreateViewModel.Name + exerciseCreateViewModel.Description),
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
            if (await _exerciseRepository.CreateExerciseAsync(exercise))
                return exercise.Id;
            return "";
        }
        public async Task<Exercise> GetExerciseByIdAsync(string idExercise)
        { 
            return await _exerciseRepository.GetExerciseAsync(idExercise);
        }



        private static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.HMACSHA256();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        
    }
}
