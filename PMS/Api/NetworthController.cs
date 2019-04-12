using Microsoft.AspNetCore.Mvc;
using PMS.Viewmodel;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace PMS.Api
{
    [Route("api/Networth")]
    public class NetworthController : Controller
    {
        // GET: api/networth/getLatestNetworh
        [HttpGet]
        [Route("GetLatestNetwoth")]
        public Networth GetLatestNetwoth()
        {
            var networthJson = System.IO.File.ReadAllText("data/Networth.json");
            var networth = JsonConvert.DeserializeObject<List<Networth>>(networthJson);

            return networth.FirstOrDefault();
        }

        // GET: api/networth/getAllNetworh
        [HttpGet]
        [Route("GetAllNetwoth")]
        public List<NetworthViewModel> GetAllNetwoth()
        {
            var networthJson = System.IO.File.ReadAllText("data/Networth.json");
            var networth = JsonConvert.DeserializeObject<List<Networth>>(networthJson);

            return networth.Select(n => new NetworthViewModel
            {
                NetworthId = n.NetworthId,
                FivePaisa = n.FivePaisa,
                Icici = n.Icici,
                IIM = n.IIM,
                Loan = n.Loan,
                Month = n.Date.Month.ToString() + "-" + n.Date.Year.ToString(),
                StockNote = n.Samco,
                Upstox = n.Upstox,
                Zerodha = n.Zerodha,
                Total = (n.FivePaisa + n.Icici + n.IIM + n.Samco + n.Upstox + n.Zerodha) - n.Loan
            }).ToList();
        }

        [HttpPost]
        public void Post([FromBody]Networth networth)
        {
            var networthJson = System.IO.File.ReadAllText("data/Networth.json");
            var networths = JsonConvert.DeserializeObject<List<Networth>>(networthJson);

            if (!networths.Any())
                networth.NetworthId = 1;
            else
            {
                var maxId = networths.Max(p => p.NetworthId);
                networth.NetworthId = maxId + 1;
            }

            networths.Add(networth);
            var serialized = JsonConvert.SerializeObject(networths);
            System.IO.File.WriteAllText("data/Networth.json", serialized);
        }
    }
}
