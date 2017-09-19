using System;
using System.Collections.Generic;
using System.Web.Http;
using business_opportunity.Models;
using business_opportunity.Persistence;

namespace business_opportunity.Controllers
{
    /// <summary>
    /// TODO: this goes to the help page
    /// </summary>
    public class OpportunityController : ApiController
    {
        private readonly IOpportunityRepository opportunityRepository;

        public OpportunityController(IOpportunityRepository opportunityRepository)
        {
            this.opportunityRepository = opportunityRepository;
        }

        /// <summary>
        /// TODO: this goes to the help page
        /// </summary>
        public IHttpActionResult GetOpportunity(int id)
        {
            Opportunity result = opportunityRepository.GetOpportunity(id);
            return Ok(result);
        }

        /// <summary>
        /// TODO: this goes to the help page
        /// </summary>
        public IHttpActionResult GetAllOpportunities()
        {
            OpportunityList result = opportunityRepository.GetAllOpportunities();
            return Ok(result);
        }
    }
}
