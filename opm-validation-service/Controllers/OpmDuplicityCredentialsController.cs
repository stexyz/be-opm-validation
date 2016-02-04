using System.Net;
using opm_validation_service.Exceptions;
using opm_validation_service.Models;
using opm_validation_service.Services;
using System;
using System.Web.Http;

namespace opm_validation_service.Controllers
{
    [Authorize]
    public class OpmDuplicityCredentialsController : AbstractOpmDuplicityController
    {
        public OpmDuplicityCredentialsController(IOpmVerificator opmVerificator, IUserAccessService userAccessService): base(opmVerificator, userAccessService)
        {
        }

        public OpmVerificationResult Get(String id)
        {
            try
            {
                OpmVerificationResult result = _opmVerificator.VerifyOpm(id, User.Identity.Name);
                _userAccessService.RecordAccess(User.Identity.Name, id, result.Result.ToString());
                return result;
            } catch (UnauthorizedAccessException) {
                _userAccessService.RecordAccess(User.Identity.Name, id, "Unauthorized.");
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Access denied due to an invalid token.");
            } catch (UserAccessLimitViolationException) {
                _userAccessService.RecordAccess(User.Identity.Name, id, "Access limitation violation.");
                ThrowHttpResponseException(HttpStatusCode.Forbidden, "Access denied due to access limit violation.");
            } catch (EanEicCodeInvalidException) {
                _userAccessService.RecordAccess(User.Identity.Name, id, "Invalid code.");
                ThrowHttpResponseException(HttpStatusCode.BadRequest, "The supplied code is not valid.");
            }
            // this return statement is required by compiler; prefer to have it here rather than inline the ThrowHttpResponseException method
            return null;
        }
    }
}
