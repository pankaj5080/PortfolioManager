using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMS.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Api
{
    [Route("api/Portfolio")]
    public class PortfolioController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<PortfolioViewModel> Get()
        {
            var portfolioJson = System.IO.File.ReadAllText("data/Portfolio.json");
            var portfolioList =  JsonConvert.DeserializeObject<List<PortfolioViewModel>>(portfolioJson);

            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var tradesList = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            portfolioList.ForEach(portfolio =>
            {
                portfolio.NumberOfTrades = tradesList
                .Count(x => x.PortfolioId == portfolio.PortfolioId && x.SellDate.HasValue);
                portfolio.Profit = tradesList.Where(x => x.PortfolioId == portfolio.PortfolioId).Sum(x => x.Profit);
                portfolio.PortfolioTypeDescription = portfolio.PortfolioTypeId == 1 ? "SingleShot" : "Position";
            });

            return portfolioList;
        }

        // GET api/portfolio/5
        [HttpGet("{id}")]
        public PortfolioViewModel Get(int id)
        {
            var portfolioJson = System.IO.File.ReadAllText("data/Portfolio.json");
            var portfolioList = JsonConvert.DeserializeObject<List<PortfolioViewModel>>(portfolioJson);

            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var tradesList = JsonConvert.DeserializeObject<List<Trade>>(tradeJson);

            var fundJson = System.IO.File.ReadAllText("data/Fund.json");
            var fundList = JsonConvert.DeserializeObject<List<Fund>>(fundJson);

            var portfolio = portfolioList.FirstOrDefault(p => p.PortfolioId == id);

            portfolio.CompletedTrades = tradesList
                .Where(x => x.PortfolioId == id && x.SellDate.HasValue && x.SellPrice.HasValue)
                .Select(trade => new TradeViewModel() {
                    BuyDate = trade.BuyDate,
                    BuyPrice = Math.Round(trade.BuyPrice, 2),
                    PortfolioId = trade.PortfolioId,
                    Profit = Math.Round((trade.SellPrice.Value - trade.BuyPrice) * trade.Quantity, 2),
                    Quantity = trade.Quantity,
                    SellDate = trade.SellDate,
                    SellPrice = Math.Round(trade.SellPrice.Value, 2),
                    StockName = trade.StockName,
                    TradeId = trade.TradeId,
                    HoldingDays = (trade.SellDate.Value - trade.BuyDate).TotalDays,
                    PercentageGain = Math.Round(((trade.SellPrice.Value - trade.BuyPrice) / trade.BuyPrice) * 100, 2)
                }).ToList();

            portfolio.OpenPosition = tradesList.Where(x => x.PortfolioId == id && !x.SellDate.HasValue)
                .Select(trade => new TradeViewModel()
                {
                    BuyDate = trade.BuyDate,
                    BuyPrice = Math.Round(trade.BuyPrice, 2),
                    Quantity = trade.Quantity,
                    StockName = trade.StockName,
                    TradeId = trade.TradeId
                }).ToList();

            var addedFunds = fundList.Where(f => f.PortfolioId == id).Sum(f => f.Amount);
            var profit = Math.Round(tradesList.Where(t => t.PortfolioId == id && t.SellDate.HasValue)
                .Sum(t => ((t.SellPrice.Value - t.BuyPrice) * t.Quantity)), 2);

            portfolio.PositionValue = Math.Round(portfolio.Positions.HasValue 
                ? ((portfolio.InitialAmount + addedFunds + profit) / portfolio.Positions.Value)
                : (portfolio.InitialAmount + addedFunds + profit), 2);

            portfolio.Profit = profit;
            portfolio.AccountValue = portfolio.InitialAmount + addedFunds + profit;
            portfolio.PercentageGain = Math.Round(portfolio.Profit / portfolio.AccountValue * 100, 2);

            return portfolio;
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]Portfolio portfolio)
        {
            var portfolioJson = System.IO.File.ReadAllText("data/Portfolio.json");
            var portfolios = JsonConvert.DeserializeObject<List<Portfolio>>(portfolioJson);

            if (!portfolios.Any())
                portfolio.PortfolioId = 1;
            else
            {
                var maxId = portfolios.Max(p => p.PortfolioId);
                portfolio.PortfolioId = maxId + 1;
            }

            portfolios.Add(portfolio);
            var serialized = JsonConvert.SerializeObject(portfolios);
            System.IO.File.WriteAllText("data/Portfolio.json", serialized);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("GetProfitDetails")]
        public ProfitAndLossViewModel GetProfitDetails(string financialYear, int portfolioId = 0)
        {
            var tradeJson = System.IO.File.ReadAllText("data/Trade.json");
            var tradesList = JsonConvert.DeserializeObject<List<Trade>>(tradeJson)
                .Where(x => x.SellDate.HasValue);

            if (portfolioId != 0)
                tradesList = tradesList
                    .Where(x => portfolioId != 0 ? x.PortfolioId == portfolioId : true)
                    .ToList();

            var startYear = Convert.ToInt32(financialYear.Substring(0, financialYear.IndexOf('-')));
            var endYear = Convert.ToInt32("20" + financialYear.Substring(financialYear.IndexOf('-') + 1));

            return new ProfitAndLossViewModel()
            {
                Apr = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 4, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 4, 30, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                May = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 5, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 5, 31, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Jun = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 6, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 6, 30, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Jul = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 7, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 7, 31, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Aug = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 8, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 8, 31, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Sep = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 9, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 9, 30, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Oct = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 10, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 10, 31, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Nov = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 11, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 11, 30, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Dec = tradesList.Where(x => x.SellDate.Value >= new DateTime(startYear, 12, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(startYear, 12, 31, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Jan = tradesList.Where(x => x.SellDate.Value >= new DateTime(endYear, 1, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(endYear, 1, 31, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Feb = tradesList.Where(x => x.SellDate.Value >= new DateTime(endYear, 2, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(endYear, 2, 28, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),
                Mar = tradesList.Where(x => x.SellDate.Value >= new DateTime(endYear, 3, 1, 0, 0, 0) &&
                    x.SellDate.Value <= new DateTime(endYear, 3, 31, 23, 59, 59))
                    .Sum(x => (x.SellPrice.Value - x.BuyPrice) * x.Quantity),   
            };
        }
    }
}
