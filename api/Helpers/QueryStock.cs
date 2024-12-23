using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryStock
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string IndustryCode { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        public string SortBy { get; set; } = string.Empty;
    }
}