﻿

namespace AnalyticsTools
{
    class Compaign
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
