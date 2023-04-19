namespace GammingManagementSystem.Models
{
    public class Campaigns
    {
        public int Id { get; set; }

        public string CampaignName { get; set; }

        public string Description { get; set; }

        public string CampaignImage { get; set; }

        public string Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int MinTarget { get; set; }
    }
}
