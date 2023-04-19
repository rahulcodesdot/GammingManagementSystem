using GammingManagementSystem.Constants;
using GammingManagementSystem.Data;
using GammingManagementSystem.Dtos;
using GammingManagementSystem.Models;
using GammingManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GammingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUserDto> applicationUsers { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CustomerList()
        {
            applicationUsers = new List<ApplicationUserDto>();
            applicationUsers = (from x in _context.ApplicationUser
                                join y in _context.UserRoles on x.Id equals y.UserId
                                join z in _context.Roles on y.RoleId equals z.Id
                                where z.Name == "USER"
                                orderby x.Id descending
                                select new ApplicationUserDto
                                {
                                    Id = x.Id,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    DateOfBirth = x.DateOfBirth,
                                    ProfilePicture = x.ProfilePicture,
                                    ReferalCode = x.ReferalCode,
                                    RegistrationDate = x.RegistrationDate,
                                    GameInterested = x.GameInterested

                                }).ToList();

            var gameList = _context.Games.ToList();
            foreach (var dtRow in applicationUsers)
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
                            games = $"<b>({index})</b>  " + gameList.Where(x => x.Id == Convert.ToInt32(arrY[i])).Select(x => x.GameName).FirstOrDefault();
                        }
                        else
                        {
                            games += $"<br/><b>({index})</b>  " + gameList.Where(x => x.Id == Convert.ToInt32(arrY[i])).Select(x => x.GameName).FirstOrDefault();
                        }
                        index++;
                    }
                    dtRow.GameInterested = games;
                }
            }

            return View(applicationUsers);
        }

        [HttpGet]
        public IActionResult GetCustomer(string id)
        {
            var response = new ApplicationUser();
            //ViewBag.GamesList = _gameService.GetGames();

            if (id != null)
            {
                response = _context.ApplicationUser.Where(x => x.Id == id).FirstOrDefault();
            }
            return View(response);
        }

        [HttpPost]
        public IActionResult DeleteCustomer(string id)
        {
            var response = _context.ApplicationUser.Where(x => x.Id == id).FirstOrDefault();

            _context.ApplicationUser.Remove(response);
            _context.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(ApplicationUser agentData)
        {
            try
            {
                var customer = new ApplicationUser();
                customer = agentData;
                customer.UserName = customer.Email;
                customer.RegistrationDate = DateTime.Now;
                var password = customer.FirstName + "@" + DateTime.Now.Year.ToString();
                var result = await _userManager.CreateAsync(customer, password);
                await _userManager.AddToRoleAsync(agentData, Roles.User.ToString());

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult UpdateCustomer(ApplicationUser agentData)
        {
            try
            {
                var updateData = _context.ApplicationUser.Where(x => x.Id == agentData.Id).FirstOrDefault();
                updateData.FirstName = agentData.FirstName;
                updateData.LastName = agentData.LastName;
                updateData.AccountName = agentData.AccountName;
                updateData.Email = agentData.Email;
                updateData.PhoneNumber = agentData.PhoneNumber;
                updateData.Address1 = agentData.Address1;
                updateData.City = agentData.City;
                updateData.State = agentData.State;

                _context.ApplicationUser.Update(updateData);
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
