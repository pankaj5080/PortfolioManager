using Microsoft.AspNetCore.Mvc;
using PMS.Viewmodel;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace PMS.Api
{
    [Route("api/Fund")]
    public class FundController : Controller
    {
        // GET: api/fund
        [HttpPost]
        public void Post([FromBody] Fund fund)
        {
            var fundsJson = System.IO.File.ReadAllText("data/Portfolio.json");
            var funds = JsonConvert.DeserializeObject<List<Fund>>(fundsJson);

            if (!funds.Any())
                fund.FundId = 1;
            else
            {
                var maxId = funds.Max(p => p.FundId);
                fund.FundId = maxId + 1;
            }

            funds.Add(fund);
            var serialized = JsonConvert.SerializeObject(funds);
            System.IO.File.WriteAllText("data/Fund.json", serialized);
        }
    }
}
