using Heroes.Models;
using Heroes.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heroes.Repositories
{
    public class HeroRepository : IHeroRepository
    {
        private readonly ApplicationDbContext _context;

        public HeroRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hero>> GetAllHeroesAsync()
        {
            return await _context.Heroes.ToListAsync();
        }

        public async Task<Hero> GetHeroByIdAsync(int id)
        {
            return await _context.Heroes.FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Hero>> GetHeroesSortedByPowerAsync()
        {
            return await _context.Heroes.OrderByDescending(h => h.CurrentPower).ToListAsync();
        }

        public async Task UpdateHeroAsync(Hero hero)
        {
            _context.Entry(hero).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
