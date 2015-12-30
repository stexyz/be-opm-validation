using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using opm_validation_service.Exceptions;
using opm_validation_service.Models;
using opm_validation_service.Services;

namespace opm_validation_service.Controllers
{
    public class OpmDuplicityController : ApiController
    {
        /// <summary>
        /// TODO SP: 
        /// 1) expose https endpoint (configurable in Web.config)
        /// </summary>
        private readonly IOpmVerificator _opmVerificator;

        private readonly string ssoCookieName = System.Configuration.ConfigurationManager.AppSettings["ssoCookieName"];


        public OpmDuplicityController(IOpmVerificator opmVerificator)
        {
            _opmVerificator = opmVerificator;
        }
        
        public OpmVerificationResult Get(String id)
        {
            CookieHeaderValue token = Request.Headers.GetCookies(ssoCookieName).FirstOrDefault();
            if (token == null)
            {
                HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) {
                    Content = new StringContent("Access denied due to invalid token.")
                };
                throw new HttpResponseException(msg);
            }
            return Get(id, token[ssoCookieName].Value);
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
            catch (UserAccessLimitViolationException)
            {
                HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Access denied due to access limit violation.")
                    };
                throw new HttpResponseException(msg);
            }
            catch (EanEicCodeInvalidException)
            {
                HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("The supplied code is not valid.")
                    };
                throw new HttpResponseException(msg);
            }
        }
    }
}
