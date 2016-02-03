using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace opm_validation_service.Controllers
{
    [Authorize]
    public class OpmDuplicityBasicController : ApiController
    {
        public String Get(String id)
        {
            return "Current Identity: " + Thread.CurrentPrincipal.Identity.Name;
        }
    }
}
