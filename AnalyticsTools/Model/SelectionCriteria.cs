using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsTools
{
    public class SelectionCriteria
    {
        public long[] Ids { get; set; } = new long[] { };
        public string[] Types { get; set; } = new string[] { };
        public string[] States { get; set; } = new string[] { };
        public string[] Statuses { get; set; } = new string[] { };
        public string[] StatusesPayment { get; set; } = new string[] { };

        public static class TYPES
        {
            public const string TEXT_CAMPAIGN = "TEXT_CAMPAIGN";
            public const string MOBILE_APP_CAMPAIGN = "MOBILE_APP_CAMPAIGN";
            public const string DYNAMIC_TEXT_CAMPAIGN = "DYNAMIC_TEXT_CAMPAIGN";
        }

        public static class STATES
        {
            public const string ARCHIVED = "ARCHIVED";
            public const string CONVERTED = "CONVERTED";
            public const string ENDED = "ENDED";
            public const string OFF = "OFF";
            public const string ON = "ON";
            public const string SUSPENDED = "SUSPENDED";
        }

        public static class STATUSES
        {
            public const string ACCEPTED = "ACCEPTED";
            public const string DRAFT = "DRAFT";
            public const string MODERATION = "MODERATION";
            public const string REJECTED = "REJECTED";
        }

        public static class STATUSESPAYMENT
        {
            public const string DISALLOWED = "DISALLOWED";
            public const string ALLOWED = "ALLOWED";
        }
    }
}
