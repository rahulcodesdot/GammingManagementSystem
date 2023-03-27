using Microsoft.AspNetCore.Identity;

namespace GammingManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string EmailId { get; set; }

        public string MobileNo { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public string ProfilePicture { get; set; }

        public string ReferalCode { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
