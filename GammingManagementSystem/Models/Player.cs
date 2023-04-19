namespace GammingManagementSystem.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string GameInterested { get; set; }

        public string PlayerImage { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string ReferralCode { get; set; }

    }
}
