using GammingManagementSystem.Data;
using GammingManagementSystem.Dtos;
using GammingManagementSystem.Models;
using GammingManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System;
using System.Security.Cryptography;
using System.Text;

namespace GammingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PlayerDto> players { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PlayerList()
        {
            players = new List<PlayerDto>();
            players = (from x in _context.Players
                       join y in _context.ApplicationUser on x.ReferralCode equals y.ReferalCode into gj
                       from subpet in gj.DefaultIfEmpty()
                       select new PlayerDto
                       {
                           Id = x.Id,
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           DateOfBirth = x.DateOfBirth,
                           ReferralCode = subpet.ReferalCode,
                           GameInterested = x.GameInterested,
                           AgentName = subpet.FirstName + " " + subpet.LastName,
                       }).ToList();

            var gameList = _context.Games.ToList();
            foreach (var dtRow in players)
            {
                string games = string.Empty;

                if (dtRow.GameInterested != null)
                {
                    var arrY = dtRow.GameInterested.Split(",");
                    ApplicationUserDto customer = new ApplicationUserDto();
                    int index = 1;
                    for (int i = 0; i < arrY.Length; i++)
                    {
                        if (games == null || games == "")
                        {
                            games = $"({index})  " + gameList.Where(x => x.Id == Convert.ToInt32(arrY[i])).Select(x => x.GameName).FirstOrDefault();
                        }
                        else
                        {
                            games += $"({index})" + gameList.Where(x => x.Id == Convert.ToInt32(arrY[i])).Select(x => x.GameName).FirstOrDefault();
                        }
                        index++;
                    }
                    dtRow.GameInterested = games;
                }
            }

            return View(players);
        }

        [HttpGet]
        public IActionResult GetPlayer(int id)
        {
            var response = new Player();
            ViewBag.GameList = _context.Games.ToList();

            if (id > 0)
            {
                response = _context.Players.Where(x => x.Id == id).FirstOrDefault();
            }
            return View(response);
        }

        [HttpPost]
        public IActionResult DeletePlayer(int id)
        {
            var response = _context.Players.Where(x => x.Id == id).FirstOrDefault();

            _context.Players.Remove(response);
            _context.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer(Player playerData)
        {
            try
            {
                var player = new Player();
                player = playerData;

                ViewBag.GameList = _context.Games.ToList();
                using var hmac = new HMACSHA512();
                var playerDT = new Player()
                {
                    FirstName = player.FirstName,
                    LastName = player.LastName,
                    ReferralCode = player.ReferralCode,
                    DateOfBirth = player.DateOfBirth,
                    GameInterested = player.GameInterested,
                    UserName = player.UserName,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(playerData.FirstName + "@" + DateTime.Now.Year)),
                    PasswordSalt = hmac.Key
                };
                _context.Players.Add(playerDT);
                _context.SaveChanges();

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult UpdatePlayer(Player playerData)
        {
            try
            {
                var updateData = _context.Players.Where(x => x.Id == playerData.Id).FirstOrDefault();
                updateData.FirstName = playerData.FirstName;
                updateData.LastName = playerData.LastName;
                updateData.DateOfBirth = playerData.DateOfBirth;
                updateData.GameInterested = playerData.GameInterested;
                updateData.UserName = playerData.UserName;

                _context.Players.Update(updateData);
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
