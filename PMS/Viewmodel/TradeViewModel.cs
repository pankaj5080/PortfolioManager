using System;

namespace PMS.Viewmodel
{
    public class TradeViewModel
    {
        public int TradeId { get; set; }
        public int PortfolioId { get; set; }
        public string StockName { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime? SellDate { get; set; }
        public double BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public double Quantity { get; set; }
        public double Profit { get; set; }
        public double PercentageGain { get; set; }
        public double HoldingDays { get; set; }
    }
}
