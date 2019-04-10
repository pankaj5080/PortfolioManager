namespace PMS.Viewmodel
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public int PortfolioTypeId { get; set; }
        public long InitialAmount { get; set; }
        public int? Position { get; set; }
    }
}
