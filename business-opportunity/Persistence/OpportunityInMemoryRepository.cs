using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using business_opportunity.Models;

namespace business_opportunity.Persistence
{
    public class OpportunityInMemoryRepository : IOpportunityRepository
    {
        public OpportunityList GetAllOpportunities()
        {
            return new OpportunityList(new List<OpportunityListItem> { new OpportunityListItem(2, "status", "type"), new OpportunityListItem(3, "status", "type"), new OpportunityListItem(1, "status", "type") }, 3);
        }

        public Opportunity GetOpportunity(int id)
        {
            return new Opportunity(id, "status", "type", new List<OpportunityDetail>() { new OpportunityDetail(1, "detail 1", DateTime.Now, 42), new OpportunityDetail(2, "detail 2", DateTime.Now, 42), new OpportunityDetail(3, "detail 3", DateTime.Now, 42) });
        }
    }
}