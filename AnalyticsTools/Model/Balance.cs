

namespace AnalyticsTools
{
    class Balance
    {
        public string CampaignId { get; set; }
        public string Sum { get; set; }
        public string Rest { get; set; }

        public override string ToString()
        {
            return "Sum: " + Sum + ", Rest: " + Rest;
        }
    }
}
