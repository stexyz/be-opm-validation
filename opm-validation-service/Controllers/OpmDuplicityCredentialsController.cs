using System.Net;
using System.Net.Http;
using opm_validation_service.Exceptions;
using opm_validation_service.Models;
using opm_validation_service.Services;
using System;
using System.Web.Http;

namespace opm_validation_service.Controllers
{
    public abstract class AbstractOpmDuplicityController : ApiController
    {
        protected readonly IOpmVerificator _opmVerificator;

        protected AbstractOpmDuplicityController(IOpmVerificator opmVerificator)
        {
            _opmVerificator = opmVerificator;
        }
        
        // ApiController will wrap the exception to the http status code and message
        protected static void ThrowHttpResponseException(HttpStatusCode statusCode, string content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(content)
                };
            throw new HttpResponseException(msg);
        }
    }

    [Authorize]
    public class OpmDuplicityCredentialsController : AbstractOpmDuplicityController
    {
        public OpmDuplicityCredentialsController(IOpmVerificator opmVerificator) : base(opmVerificator)
        {
        }

        public OpmVerificationResult Get(String id)
        {
            try
            {
                return _opmVerificator.VerifyOpm(id, User.Identity.Name);
            } catch (UnauthorizedAccessException) {
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Access denied due to an invalid token.");
            } catch (UserAccessLimitViolationException) {
                ThrowHttpResponseException(HttpStatusCode.Forbidden, "Access denied due to access limit violation.");
            } catch (EanEicCodeInvalidException) {
                ThrowHttpResponseException(HttpStatusCode.BadRequest, "The supplied code is not valid.");
            }
            // this return statement is required by compiler; prefer to have it here rather than inline the ThrowHttpResponseException method
            return null;
        }
    }
}
