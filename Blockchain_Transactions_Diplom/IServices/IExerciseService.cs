using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IServices
{
    public interface IExerciseService
    {
        public Task<bool> CreateExerciseAsync(ExerciseCreateViewModel exerciseCreateViewModel);
        public Task<Exercise> GetExerciseByIdAsync(int idExercise);
    }
}
