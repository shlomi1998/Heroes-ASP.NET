using System.Collections.Generic;
using System.Threading.Tasks;
using Heroes.Models;

namespace Heroes.Repositories
{
    public interface ITrainerRepository
    {
        Task<IEnumerable<Trainer>> GetAllTrainersAsync();
        Task<Trainer> GetTrainerByIdAsync(int id);
        Task<Trainer> AddTrainerAsync(Trainer trainer);
        Task<Trainer> UpdateTrainerAsync(Trainer trainer);
        Task DeleteTrainerAsync(int id);
        Task<Trainer> GetTrainerByUsernameAsync(string username);
        Task<bool> IsUsernameExistsAsync(string username);
    }
}
