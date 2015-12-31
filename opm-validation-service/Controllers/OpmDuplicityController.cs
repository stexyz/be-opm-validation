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

        private readonly string _ssoCookieName = System.Configuration.ConfigurationManager.AppSettings["ssoCookieName"];

        public OpmDuplicityController(IOpmVerificator opmVerificator)
        {
            _opmVerificator = opmVerificator;
        }
        
        public OpmVerificationResult Get(String id)
        {
            CookieHeaderValue token = Request.Headers.GetCookies(_ssoCookieName).FirstOrDefault();
            if (token == null)
            {
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Access denied due to invalid token.");
            }
            return Get(id, token[_ssoCookieName].Value);
        }

        public OpmVerificationResult Get(String id, String token) {
            try
            {
                return _opmVerificator.VerifyOpm(id, token);
            }
            catch (UnauthorizedAccessException)
            {
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Access denied due to an invalid token.");
            }
            catch (UserAccessLimitViolationException)
            {
                ThrowHttpResponseException(HttpStatusCode.Forbidden, "Access denied due to access limit violation.");
            }
            catch (EanEicCodeInvalidException)
            {
                ThrowHttpResponseException(HttpStatusCode.BadRequest, "The supplied code is not valid.");
            }
            // this return statement is required by compiler; prefer to have it here rather than inline the ThrowHttpResponseException method
            return null;
        }

        // ApiController will wrap the exception to the http status code and message
        private static void ThrowHttpResponseException(HttpStatusCode statusCode, string content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(content)
                };
            throw new HttpResponseException(msg);
        }
    }
}
