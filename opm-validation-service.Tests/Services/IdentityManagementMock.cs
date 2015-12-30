using opm_validation_service.Services;

namespace opm_validation_service.Tests.Services {
    public class IdentityManagementMock : IIdentityManagement {
        public bool ValidateUser(string token)
        {
            return token == "valid";
        }

        public IUser GetUserInfo(string token)
        {
            return token == "valid" ? new User("valid") : null;
        }

        public string Login(string userName, string password)
        {
            return userName == "valid" && password == "valid" ? "valid" : null;

        }
    }
}
