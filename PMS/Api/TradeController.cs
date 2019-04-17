using Microsoft.AspNetCore.Mvc;
using PMS.Viewmodel;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace PMS.Api
{
    [Route("api/Trade")]
    public class TradeController : Controller
    {
        // GET: api/fund
        [HttpPost]
        public void Post([FromBody] Trade trade)
        {
            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var trades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            if (trade.TradeId == 0)
            {
                if (!trades.Any())
                    trade.TradeId = 1;
                else
                {
                    var maxId = trades.Max(p => p.TradeId);
                    trade.TradeId = maxId + 1;
                }
                trades.Add(trade);
            }
            else
            {
                var currentTrade = trades.FirstOrDefault(t => t.TradeId == trade.TradeId);
                currentTrade.SellDate = trade.SellDate;
                currentTrade.SellPrice = trade.SellPrice;
            }

            var serialized = JsonConvert.SerializeObject(trades);
            System.IO.File.WriteAllText("data/Trade.json", serialized);
        }

        [HttpGet]
        [Route("GetOpenPosition")]
        public List<Trade> GetOpenPosition(int portfolioId)
        {
            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var trades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            return trades.Where(x => x.PortfolioId == portfolioId && !x.SellDate.HasValue).ToList();
        }

        [HttpGet]
        [Route("GetTrade")]
        public Trade GetTrade(int tradeId)
        {
            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var trades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            return trades.FirstOrDefault(x => x.TradeId == tradeId);
        }

        [HttpGet]
        [Route("GetPortfolioHistoricalTrades")]
        public List<Trade> GetPortfolioHistoricalTrades(int portfolioId)
        {
            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var trades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            return trades.Where(x => x.PortfolioId == portfolioId).ToList();
        }

        [HttpGet]
        [Route("GetPortfolioCompletedTrades")]
        public List<Trade> GetPortfolioCompletedTrades(int portfolioId)
        {
            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var trades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            return trades.Where(x => x.PortfolioId == portfolioId && x.SellDate.HasValue).ToList();
        }

        [HttpGet]
        [Route("GetCompletedTrades")]
        public List<Trade> GetCompletedTrades()
        {
            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var trades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            return trades.Where(x => x.SellDate.HasValue).ToList();
        }

        [HttpGet]
        [Route("GetDashboardDetails")]
        public Dashboard GetDashboardDetails()
        {
            var networthJson = System.IO.File.ReadAllText("data/Networth.json");
            var latestNetworth = JsonConvert.DeserializeObject<List<Networth>>(networthJson);

            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var completedTrades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson)
                .Where(t => t.SellDate.HasValue)
                .OrderByDescending(t => t.SellDate.Value)
                .Select(t => new Trade
                {
                    BuyDate = t.BuyDate,
                    BuyPrice = t.BuyPrice,
                    PortfolioId = t.PortfolioId,
                    Quantity = t.Quantity,
                    SellDate = t.SellDate.Value,
                    SellPrice = t.SellPrice.Value,
                    StockName = t.StockName,
                    TradeId = t.TradeId,
                    Profit = Math.Round((t.SellPrice.Value - t.BuyPrice) * t.Quantity, 2)
                }) ;

            var qtrMinDate = GetQuaterMinDate();
            var qtrMaxDate = GetQuaterMaxDate();
            var qtrProfits = completedTrades
                .Where(t => t.SellDate.HasValue && t.SellDate > qtrMinDate && t.SellDate < qtrMaxDate)
                .Sum(t => (t.SellPrice.Value - t.BuyPrice) * t.Quantity);

            var yearMinDate = GetYearMinDate();
            var yearMaxDate = GetYearMaxDate();
            var yearProfits = completedTrades
                .Where(t => t.SellDate.HasValue && t.SellDate > yearMinDate && t.SellDate < yearMaxDate)
                .Sum(t => (t.SellPrice.Value - t.BuyPrice) * t.Quantity);

            var overallProfits = completedTrades
                .Where(t => t.SellDate.HasValue)
                .Sum(t => (t.SellPrice.Value - t.BuyPrice) * t.Quantity);

            var tradingFunds = GetTradingFunds();

            var networths = latestNetworth.Select(networth => new NetworthDashboard
            {
                Month = networth.Date.Month.ToString() + "-" + networth.Date.Year.ToString(),
                FivePaisa = networth.FivePaisa,
                Icici = networth.Icici,
                Iim = networth.IIM,
                Samco = networth.Samco,
                Upstox = networth.Upstox,
                Zerodha = networth.Zerodha,
                Loan = networth.Loan,
                Total = networth.FivePaisa + networth.Icici + networth.IIM +
                networth.Samco + networth.Upstox + networth.Zerodha - networth.Loan
            }).ToList();

            return new Dashboard
            {
                AllTrades = completedTrades.ToList(),
                LastFiveTrades = completedTrades.Take(10).ToList(),
                OverallProfit = Math.Round(overallProfits, 2),
                QuarterProfit = Math.Round(qtrProfits, 2),
                TradingFunds = Math.Round(tradingFunds, 2),
                YearProfit = Math.Round(yearProfits, 2),
                Networths = networths
            };
        }

        public DateTime GetQuaterMinDate()
        {
            var date = DateTime.Now;
            if (date.Month >= 1 && date.Month < 4)
                return new DateTime(date.Year, 1, 1, 0, 0, 0);
            if (date.Month > 3 && date.Month < 7)
                return new DateTime(date.Year, 4, 1, 0, 0, 0);
            if (date.Month > 6 && date.Month < 10)
                return new DateTime(date.Year, 7, 1, 0, 0, 0);

            return new DateTime(date.Year, 10, 1, 0, 0, 0);
        }
        public DateTime GetQuaterMaxDate()
        {
            var date = DateTime.Now;
            if (date.Month >= 1 && date.Month < 4)
                return new DateTime(date.Year, 3, 31, 23, 59, 59);
            if (date.Month > 3 && date.Month < 7)
                return new DateTime(date.Year, 6, 30, 23, 59, 59);
            if (date.Month > 6 && date.Month < 10)
                return new DateTime(date.Year, 9, 30, 23, 59, 59);

            return new DateTime(date.Year, 12, 31, 23, 59, 59);
        }

        public DateTime GetYearMinDate()
        {
            var date = DateTime.Now;
            if (date.Month < 4)
                return new DateTime(date.Year - 1, 4, 1, 0, 0, 0);

            return new DateTime(date.Year, 4, 1, 0, 0, 0);
        }

        public DateTime GetYearMaxDate()
        {
            var date = DateTime.Now;
            if (date.Month < 4)
                return new DateTime(date.Year, 3, 31, 23, 59, 59);

            return new DateTime(date.Year + 1, 3, 31, 23, 59, 59);
        }

        public double GetTradingFunds()
        {
            var portfolioJson = System.IO.File.ReadAllText("data/Portfolio.json");
            var portfolios = JsonConvert.DeserializeObject<List<Portfolio>>(portfolioJson);

            var fundJson = System.IO.File.ReadAllText("data/Fund.json");
            var fundList = JsonConvert.DeserializeObject<List<Fund>>(fundJson);

            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var completedTrades = JsonConvert.DeserializeObject<List<Trade>>(tradeJson)
                .Where(t => t.SellDate.HasValue);

            var portfolioInitialFunds = portfolios.Sum(p => p.InitialAmount);
            var profits = completedTrades.Sum(t => (t.SellPrice.Value - t.BuyPrice) * t.Quantity);
            var funds = fundList.Sum(f => f.Amount);

            return portfolioInitialFunds + profits + funds;
        }

    }
}
