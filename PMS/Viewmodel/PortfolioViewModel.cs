using System.Collections.Generic;

namespace PMS.Viewmodel
{
    public class PortfolioViewModel
    {
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public int PortfolioTypeId { get; set; }
        public string PortfolioTypeDescription { get; set; }
        public long InitialAmount { get; set; }
        public int? Position { get; set; }
        public double PositionValue { get; set; }
        public double Profit { get; set; }
        public double PercentageGain { get; set; }
        public double AccountValue { get; set; }
        public int NumberOfTrades { get; set; }

        public List<TradeViewModel> OpenPosition { get; set; }
        public List<TradeViewModel> CompletedTrades { get; set; }
    }
}
