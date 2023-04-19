namespace GammingManagementSystem.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string GameInterested { get; set; }

        public string PlayerImage { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string AgentName { get; set; }

        public string ReferralCode { get; set; }

    }
}
