using Heroes.Models;
using Heroes.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heroes.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trainer>> GetAllTrainersAsync()
        {
            return await _context.Trainers.Include(t => t.Heroes).ToListAsync();
        }

        public async Task<Trainer> GetTrainerByIdAsync(int id)
        {
            return await _context.Trainers.Include(t => t.Heroes).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Trainer> AddTrainerAsync(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return trainer;
        }

        public async Task<Trainer> UpdateTrainerAsync(Trainer trainer)
        {
            _context.Entry(trainer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return trainer;
        }

        public async Task DeleteTrainerAsync(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Trainer> GetTrainerByUsernameAsync(string username)
        {
            return await _context.Trainers.SingleOrDefaultAsync(t => t.Username == username);
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _context.Trainers.AnyAsync(t => t.Username == username);
        }
    }
}
