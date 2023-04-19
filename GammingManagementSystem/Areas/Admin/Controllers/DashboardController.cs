using GammingManagementSystem.Constants;
using GammingManagementSystem.Data;
using GammingManagementSystem.Dtos;
using GammingManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GammingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public DashboardController(ApplicationDbContext context,
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

        public IActionResult AgentList()
        {
            applicationUsers = new List<ApplicationUserDto>();
            applicationUsers = (from x in _context.ApplicationUser
                                join y in _context.UserRoles on x.Id equals y.UserId
                                join z in _context.Roles on y.RoleId equals z.Id
                                where z.Name == "Agent"
                                orderby x.Id descending
                                select new ApplicationUserDto
                                {
                                    Id = x.Id,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    DateOfBirth = x.DateOfBirth,
                                    ProfilePicture = x.ProfilePicture,
                                    RegistrationDate = x.RegistrationDate,
                                }).ToList();

            return View(applicationUsers);
        }

        [HttpGet]
        public IActionResult GetAgent(string id)
        {
            var response = new ApplicationUser();
            if (id != null)
            {
                response = _context.ApplicationUser.Where(x => x.Id == id).FirstOrDefault();
            }
            return View(response);
        }

        [HttpPost]
        public IActionResult DeleteAgent(string id)
        {
            var response = _context.ApplicationUser.Where(x => x.Id == id).FirstOrDefault();

            _context.ApplicationUser.Remove(response);
            _context.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> AddAgent(ApplicationUser agentData)
        {
            try
            {
                var agent = new ApplicationUser();
                agent = agentData;
                agent.UserName = agent.Email;
                agent.RegistrationDate = DateTime.Now;
                var password = agent.FirstName + "@" + DateTime.Now.Year.ToString();
                var result = await _userManager.CreateAsync(agent, password);
                await _userManager.AddToRoleAsync(agentData, Roles.Agent.ToString());

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult UpdateAgent(ApplicationUser agentData)
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

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
