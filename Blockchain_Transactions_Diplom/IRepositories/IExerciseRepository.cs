using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;

namespace Blockchain_Transactions_Diplom.IRepositories
{
    public interface IExerciseRepository
    {
        public Task<Exercise> GetExerciseAsync(int idExercise);
        public Task<bool> CreateExerciseAsync(Exercise exercise);
        public Task<bool> DeleteExerciseAsync(int idExercise);
        public Task<Exercise> GetExerciseByIdAsync(int idExercise);
    }
}
