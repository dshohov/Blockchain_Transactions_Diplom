using Blockchain_Transactions_Diplom.Data;
using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Blockchain_Transactions_Diplom.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ApplicationDBContext _context;
        public ExerciseRepository(ApplicationDBContext context) 
        {
            _context = context;
        }

        public async Task<bool> CreateExerciseAsync(Exercise exercise)
        {
            await _context.AddAsync(exercise);
            return await SaveAsync();
        }
        public async Task<Exercise> GetExerciseAsync(int idExercise) => await _context.Exercises.FirstOrDefaultAsync(x=>x.Id == idExercise);
        public async Task<bool> DeleteExerciseAsync(int idExercise)
        {
            var exercise = await GetExerciseAsync(idExercise);
            await Task.Run(() => _context.Remove(exercise));
            return await SaveAsync();
        }
        private async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
        public async Task<Exercise> GetExerciseByIdAsync(int idExercise)
        {
            return await _context.Exercises.FindAsync(idExercise);
        }
    }
}
