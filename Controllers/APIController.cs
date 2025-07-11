using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using RaceStrategyApp.Models;
using System.Collections.Generic;

namespace RaceStrategyApp.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class APIController : ControllerBase {
        protected RaceStrategyContext Ctx { get; set; }
        public APIController() {
            Ctx = new RaceStrategyContext();
        }

        

    }
}
