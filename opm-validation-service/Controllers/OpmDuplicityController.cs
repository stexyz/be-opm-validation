using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using opm_validation_service.Models;
using opm_validation_service.Services;

namespace opm_validation_service.Controllers
{
    public class OpmDuplicityController : ApiController
    {
        /// <summary>
        /// TODO SP: 
        /// 1) expose https endpoint (configurable in Web.config)
        /// 2) decide what to do if number of allowed requests is over limit (http 4xx result vs. code in OpmVerificationResult) 
        /// </summary>
        private readonly IOpmVerificator _opmVerificator;

        public OpmDuplicityController(IOpmVerificator opmVerificator)
        {
            _opmVerificator = opmVerificator;
        }
        
        public OpmVerificationResult Get(String id)
        {
            CookieHeaderValue token = Request.Headers.GetCookies("iPlanetDirectoryPro").FirstOrDefault();
            //TODO SP: pass the token cookie to the verifyOpmCall or return 401
            return _opmVerificator.VerifyOpm(id);
        }

        public OpmVerificationResult Get(String id, String token) {
            try
            {
                return _opmVerificator.VerifyOpm(id, token);
            }
            catch (UnauthorizedAccessException)
            {
                HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("Access denied due to invalid token.")
                    };
                throw new HttpResponseException(msg);
            }
        }
    }
}
