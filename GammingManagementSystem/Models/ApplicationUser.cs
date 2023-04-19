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
        public DateTime? RegistrationDate { get; set; }
        public string ProfilePicture { get; set; }
        public string ReferalCode { get; set; }
        public string ReferalCodeBy { get; set; }
        public string AgentId { get; set; }
        public bool? IsAdmin { get; set; }
        public string AccountName { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string GameInterested { get; set; }
    }
}
