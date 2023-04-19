using GammingManagementSystem.Models;

namespace GammingManagementSystem.Services
{
    public interface IGameService
    {
        Task<List<Games>> GetGames();
    }
}
