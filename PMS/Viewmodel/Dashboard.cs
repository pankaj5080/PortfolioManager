using System.Collections.Generic;

namespace PMS.Viewmodel
{
    public class Dashboard
    {
        public double QuarterProfit { get; set; }
        public double YearProfit { get; set; }
        public double OverallProfit { get; set; }
        public double TradingFunds { get; set; }
        public List<NetworthDashboard> Networths { get; set; }
        public List<Trade> AllTrades { get; set; }
        public List<Trade> LastFiveTrades  { get; set; }
    }
}
