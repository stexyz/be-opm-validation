using System;
using System.Collections.Generic;
using System.Web.Http;
using business_opportunity.Models;

namespace business_opportunity.Controllers
{
    /// <summary>
    /// TODO: this goes to the help page
    /// </summary>
    public class OpportunityController : ApiController
    {
        /// <summary>
        /// TODO: this goes to the help page
        /// </summary>
        public IHttpActionResult GetOpportunity(int id)
        {
            Opportunity result = new Opportunity(id, "status", "type", new List<OpportunityDetail>() { new OpportunityDetail(1, "detail 1", DateTime.Now, 42), new OpportunityDetail(2, "detail 2", DateTime.Now, 42), new OpportunityDetail(3, "detail 3", DateTime.Now, 42) });
            return Ok(result);
        }

        /// <summary>
        /// TODO: this goes to the help page
        /// </summary>
        public IHttpActionResult GetAllOpportunities()
        {
            OpportunityList result = new OpportunityList(new List<OpportunityListItem> { new OpportunityListItem(2, "status", "type"), new OpportunityListItem(3, "status", "type"), new OpportunityListItem(1, "status", "type") }, 3);
            return Ok(result);
        }
    }
}
