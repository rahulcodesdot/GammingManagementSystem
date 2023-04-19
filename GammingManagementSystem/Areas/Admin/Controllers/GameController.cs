using GammingManagementSystem.Data;
using GammingManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GammingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GameList()
        {
            var game = _context.Games.OrderByDescending(x=>x.Id).ToList();
            return View(game);
        }

        public IActionResult GetGame(int id)
        {
            var game = _context.Games.Where(x => x.Id == id).FirstOrDefault();
            return View(game);
        }

        [HttpPost]
        public IActionResult DeleteGame(int id)
        {
            var response = _context.Games.Where(x => x.Id == id).FirstOrDefault();

            _context.Games.Remove(response);
            _context.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame(Games gameData)
        {
            try
            {
                var game = new Games();
                game = gameData;
                _context.Games.Add(game);
                _context.SaveChanges();

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult UpdateGame(Games gameData)
        {
            try
            {
                var updateData = _context.Games.Where(x => x.Id == gameData.Id).FirstOrDefault();
                updateData.GameName = gameData.GameName;
                _context.Games.Update(updateData);
                _context.SaveChanges(true);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
