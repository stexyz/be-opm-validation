using System.Net;
using System.Net.Http;
using System.Web.Http;
using opm_validation_service.Services;

namespace opm_validation_service.Controllers
{
    public abstract class AbstractOpmDuplicityController : ApiController
    {
        protected readonly IOpmVerificator _opmVerificator;
        protected readonly IUserAccessService _userAccessService;

        protected AbstractOpmDuplicityController(IOpmVerificator opmVerificator, IUserAccessService userAccessService)
        {
            _opmVerificator = opmVerificator;
            _userAccessService = userAccessService;
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
}