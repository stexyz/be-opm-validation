using opm_validation_service.Models;

namespace opm_validation_service.Services
{
    public interface IOpmVerificator
    {
        OpmVerificationResult VerifyOpmWithToken(string codeString, string token, out string username);
        OpmVerificationResult VerifyOpm(string codeString, string username);
    }
}