using opm_validation_service.Models;

namespace opm_validation_service.Services
{
    public interface IOpmVerificator
    {
        /// <summary>
        /// What to do if the EAN/EIC is invalid 
        /// </summary>
        /// <param name="codeString"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        OpmVerificationResult VerifyOpm(string codeString, string token);
    }
}