using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface IExerciseService
    {
        Task<string> CreateExerciseAsync(ExerciseCreateViewModel exerciseCreateViewModel);
        public Task<Exercise> GetExerciseByIdAsync(string idExercise);
    }
}
