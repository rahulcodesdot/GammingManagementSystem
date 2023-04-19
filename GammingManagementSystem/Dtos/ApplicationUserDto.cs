namespace GammingManagementSystem.Dtos
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public string ProfilePicture { get; set; }

        public string ReferalCode { get; set; }
        public string UserRole { get; set; }

        public string GameInterested { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
