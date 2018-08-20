
namespace AnalyticsTools
{
    public class Params
    {
        public SelectionCriteria SelectionCriteria { get; set; } = new SelectionCriteria();
        public string[] FieldNames { get; set; } = new string[] { };

        public static class FIELDNAMES
        {
            public const string BlockedIps = "BlockedIps";
            public const string Type = "Type";
            public const string Id = "Id";
            public const string Name = "Name";

        }
    }
}
