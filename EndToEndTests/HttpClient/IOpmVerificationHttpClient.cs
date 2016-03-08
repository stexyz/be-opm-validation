using System;
using opm_validation_service.Models;

namespace EndToEndTests.HttpClient
{
    interface IOpmVerificationHttpClient
    {
        /// <summary>
        /// Version with login IDM with cookie token.
        /// </summary>
        /// <param name="id">EAN/EIC code to validate</param>
        /// <returns>Verification result</returns>
        OpmVerificationResult GetWithCookie(String id, String token);

        /// <summary>
        /// Version with login against IDM.
        /// </summary>
        /// <param name="id">EAN/EIC code to validate</param>
        /// <param name="token">IDM tokem</param>
        /// <returns>Verification result</returns>
        OpmVerificationResult GetWithInlineToken(String id, String token);
        
        /// <summary>
        /// Version with login against AD.
        /// </summary>
        /// <param name="id">EAN/EIC code to validate</param>
        /// <returns>Verification result</returns>
        OpmVerificationResult GetAd(String id);
    }
}