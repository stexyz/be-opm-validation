using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using business_opportunity.Models;

namespace business_opportunity.Persistence
{
    public interface IOpportunityRepository
    {
        OpportunityList GetAllOpportunities();
        Opportunity GetOpportunity(int id);
    }
}
