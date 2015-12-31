using opm_validation_service.Models;
using opm_validation_service.Services;

namespace opm_validation_service.Persistence
{
    public class UserLimitInMemoryRepository: IUserLimitRepository{
        public bool TryAdd(IUser user, int count, out int currentLimit, out int currentBalance)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetUserLimit(IUser user, out int limit)
        {
            throw new System.NotImplementedException();
        }

        public bool AddOrReset(IUser user)
        {
            throw new System.NotImplementedException();
        }

        public bool TryRemoveOpm(IUser code)
        {
            throw new System.NotImplementedException();
        }
    }
}