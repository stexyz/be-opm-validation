using System;
using opm_validation_service.Exceptions;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Services {

    public class OpmVerificator : IOpmVerificator
    {
        public OpmVerificator(IIdentityManagement identityManagement, IEanEicCheckerHttpClient eanEicCheckerHttpClient, IOpmRepository opmRepository, IUserAccessService userAccessService)
        {
            IdentityManagement = identityManagement;
            EanEicCheckerHttpClient = eanEicCheckerHttpClient;
            OpmRepository = opmRepository;
            UserAccessService = userAccessService;
        }

        //BEANS:
        public IIdentityManagement IdentityManagement { private get; set; }
        public IEanEicCheckerHttpClient EanEicCheckerHttpClient { private get; set; }
        public IOpmRepository OpmRepository { private get; set; }
        public IUserAccessService UserAccessService { private get; set; }

        /// <summary>
        /// TODO SP: 
        ///   a. what to do if the EAN/EIC is invalid 
        /// </summary>
        /// <param name="codeString"></param>
        /// <returns></returns>
        private OpmVerificationResult VerifyOpm(string codeString) {
            EanEicCode code = new EanEicCode(codeString);
            return Verify(code);
        }

        public OpmVerificationResult VerifyOpm(string codeString, string token)
        {
            if (!IdentityManagement.ValidateUser(token))
            {
                throw new UnauthorizedAccessException("Access denied due to invalid token.");
            }

            IUser userInfo = IdentityManagement.GetUserInfo(token);

            if (UserAccessService.TryAccess(userInfo))
            {
                return VerifyOpm(codeString);
            }

            throw new UserAccessLimitViolationException();
        }

        private OpmVerificationResult Verify(EanEicCode code) {
            CheckResult codeValid = EanEicCheckerHttpClient.Post(code);
            if (codeValid.ResultCode != CheckResultCode.EanOk && codeValid.ResultCode != CheckResultCode.EicOk) {
                throw new EanEicCodeInvalidException();
            }

            //OK, code is valid, try to find it in the OpmRepository
            Opm opmForCode;
            if (OpmRepository.TryGetOpm(code, out opmForCode)) {
                return new OpmVerificationResult(true);
            }
            return new OpmVerificationResult(false);
        }
    }
}