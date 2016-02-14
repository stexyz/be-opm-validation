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
    /// <summary>
    /// Test help.
    /// </summary>
    public class OpmDuplicityController : AbstractOpmDuplicityController
    {
        /// <summary>
        /// TODO SP: 
        /// 1) expose https endpoint (configurable in Web.config)
        /// </summary>
        private readonly string _ssoCookieName = System.Configuration.ConfigurationManager.AppSettings["ssoCookieName"];

        public OpmDuplicityController(IOpmVerificator opmVerificator, IUserAccessService userAccessService) : base(opmVerificator, userAccessService)
        {
        }
        
        /// <summary>
        /// Test help for get.
        /// </summary>
        /// <param name="id">id param</param>
        /// <returns>returns some result..</returns>
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
            string username = null;
            try
            {
                OpmVerificationResult result = _opmVerificator.VerifyOpmWithToken(id, token, out username);
                _userAccessService.RecordAccess(username, id, result.Result.ToString());
                return result;
            } catch (UnauthorizedAccessException) {
                _userAccessService.RecordAccess(username, id, "Unauthorized.");
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Access denied due to an invalid token.");
            } catch (UserAccessLimitViolationException) {
                _userAccessService.RecordAccess(username, id, "Access limitation violation.");
                ThrowHttpResponseException(HttpStatusCode.Forbidden, "Access denied due to access limit violation.");
            } catch (EanEicCodeInvalidException) {
                _userAccessService.RecordAccess(username, id, "Invalid code.");
                ThrowHttpResponseException(HttpStatusCode.BadRequest, "The supplied code is not valid.");
            }
            // this return statement is required by compiler; prefer to have it here rather than inline the ThrowHttpResponseException method
            return null;
        }
    }
}
