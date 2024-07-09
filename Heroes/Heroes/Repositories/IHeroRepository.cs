using System.Collections.Generic;
using System.Threading.Tasks;
using Heroes.Models;

namespace Heroes.Repositories
{
    public interface IHeroRepository
    {
        Task<IEnumerable<Hero>> GetAllHeroesAsync();
        Task<Hero> GetHeroByIdAsync(int id);
        Task<IEnumerable<Hero>> GetHeroesSortedByPowerAsync();
        Task UpdateHeroAsync(Hero hero);
    }
}
