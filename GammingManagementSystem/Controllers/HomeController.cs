using GammingManagementSystem.Data;
using GammingManagementSystem.Dtos;
using GammingManagementSystem.Helper;
using GammingManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickMailer;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace GammingManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PlayerRegistration(string referralCode)
        {

            if (_signInManager.IsSignedIn(User)) 
            {
                TempData["Error"] = "User already sign in. you can not sign up.";
                return RedirectToAction("Index");
            }

            var playerDT = new PlayerDto();
            ViewBag.GameList = _context.Games.ToList();

            if (referralCode != null)
            {
                playerDT.ReferralCode = referralCode;
            }

            return View(playerDT);
        }

        [HttpPost]
        public IActionResult PlayerRegistration(PlayerDto player)
        {
            if (ModelState.IsValid == true)
            {

                var checkUserNameExist = _context.Players.Where(x => x.UserName == player.UserName).ToList();

                if (checkUserNameExist.Count > 0)
                {
                    TempData["Error"] = "Username already exist.";
                    return RedirectToAction("PlayerRegistration");
                }

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
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(player.Password)),
                    PasswordSalt = hmac.Key
                };
                _context.Players.Add(playerDT);
                _context.SaveChanges();

                var AgentEmail = checkRefAgent(player.ReferralCode);

                if (AgentEmail != null)
                {
                    var Subject = $"Player Registration-{DateTime.Now.Date}";
                    var MailMessage = $"Player added using your Referral Code:{AgentEmail.FirstName} {AgentEmail.LastName}";
                    sendMail(AgentEmail.Email, MailMessage, Subject);
                }

                TempData["Success"] = "Player register successfully";

                return RedirectToAction("PlayerRegistration");
            }
            TempData["Error"] = "Please enter valid data.";
            return RedirectToAction("PlayerRegistration");
        }

        public IActionResult CheckUserName(string userName)
        {
            UserAvilableDto userAvilableDto = new UserAvilableDto();

            while (IsUserNameExist(userName))
            {
                Random random = new Random();
                int randonNumber = random.Next(0, 6);
                userName = userName + randonNumber.ToString();

                userAvilableDto.Message = "Username is not-availble";
                userAvilableDto.AvailbleUserName = "Availble username : " + userName;
                return Json(userAvilableDto);
            }

            userAvilableDto.Message = "Username availble";
            userAvilableDto.AvailbleUserName = userName;
            return Json(userAvilableDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsUserNameExist(string userName)
        {
            try
            {
                var response = (from player in _context.Players
                                where player.UserName == userName
                                select new PlayerDto
                                {
                                    UserName = player.UserName,
                                }).ToList();


                if (response.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private ApplicationUser checkRefAgent(string refCode)
        {
            var agent = _context.ApplicationUser.
                Where(x => x.ReferalCode == refCode).FirstOrDefault();

            return agent;
        }

        private void sendMail(string toMailId, string Message, string Subject)
        {
            try
            {
                Email email = new Email();
                email.SendEmail(toMailId, Credential.Email, Credential.Password, Subject, Message);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}