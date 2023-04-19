using GammingManagementSystem.Data;
using GammingManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GammingManagementSystem.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Games>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }
    }
}
