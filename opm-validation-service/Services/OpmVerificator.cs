using System;
using opm_validation_service.Exceptions;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Services {

    //TODO SP: verificator or verifier or some other better name
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

        public OpmVerificationResult VerifyOpmWithToken(string codeString, string token) {
            if (!IdentityManagement.ValidateUser(token)) {
                throw new UnauthorizedAccessException();
            }
            string userInfo = IdentityManagement.GetUsername(token);

            return VerifyOpm(codeString, userInfo);
        }

        public OpmVerificationResult VerifyOpm(string codeString, string username)
        {
            EanEicCode code = new EanEicCode(codeString);
            if (UserAccessService.TryAccess(username, code)) {
                return VerifyOpm(code);
            }

            throw new UserAccessLimitViolationException();
        }

        private OpmVerificationResult VerifyOpm(EanEicCode code) {
            return Verify(code);
        }

        private OpmVerificationResult Verify(EanEicCode code) {
            CheckResult codeValid = EanEicCheckerHttpClient.Post(code);
            if (codeValid.ResultCode != CheckResultCode.EanOk && codeValid.ResultCode != CheckResultCode.EicOk) {
                throw new EanEicCodeInvalidException();
            }

            //OK, code is valid, try to find the record in the OpmRepository
            Opm opmForCode;
            if (OpmRepository.TryGetOpm(code, out opmForCode)) {
                return new OpmVerificationResult(true);
            }
            return new OpmVerificationResult(false);
        }
    }
}