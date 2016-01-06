using opm_validation_service.Models;
using opm_validation_service.Services;

namespace opm_validation_service.Tests.Services {
    public class IdentityManagementMock : IIdentityManagement {
        public bool ValidateUser(string token)
        {
            return token == "valid" || token == "valid2" || token == "depleated";
        }

        public IUser GetUserInfo(string token)
        {
            if (token == "valid" || token == "valid2" || token == "depleated")
            {
                return new User(token);
            }
            return null;
        }

        public string Login(string userName, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
