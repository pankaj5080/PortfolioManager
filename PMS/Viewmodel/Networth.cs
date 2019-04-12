using System;

namespace PMS.Viewmodel
{
    public class Networth
    {
        public int NetworthId { get; set; }
        public DateTime Date { get; set; }
        public double Icici { get; set; }
        public double Zerodha { get; set; }
        public double Upstox { get; set; }
        public double FivePaisa { get; set; }
        public double IIM { get; set; }
        public double Samco { get; set; }
        public double Loan { get; set; }
    }
}
