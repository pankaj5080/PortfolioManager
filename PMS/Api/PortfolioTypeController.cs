using Microsoft.AspNetCore.Mvc;
using PMS.Viewmodel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PMS.Api
{
    [Route("api/PortfolioType")]
    public class PortfolioTypeController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<PortfolioType> Get()
        {
            var portfolioTypes = System.IO.File.ReadAllText("data/PortfolioType.json");
            return JsonConvert.DeserializeObject<List<PortfolioType>>(portfolioTypes);
        }
    }
}
