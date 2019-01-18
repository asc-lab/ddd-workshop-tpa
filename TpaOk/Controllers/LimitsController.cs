using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TpaOk.Commands;

namespace TpaOk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimitsController : ControllerBase
    {
        private readonly CalculateCostSplitAndReserveLimitsHandler _costSplitAndReserveLimitsHandler;

        public LimitsController(CalculateCostSplitAndReserveLimitsHandler costSplitAndReserveLimitsHandler)
        {
            _costSplitAndReserveLimitsHandler = costSplitAndReserveLimitsHandler;
        }

        // POST api/values
        [HttpPost("/calculateAndReserveLimits")]
        public IActionResult CalculateAndReserveLimits([FromBody] CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            return new JsonResult(_costSplitAndReserveLimitsHandler.Handle(cmd));
        }

        // DELETE api/values/5
        [HttpDelete("{caseNumber}")]
        public void ReleaseLimits(string caseNumber)
        {
        }
    }
}