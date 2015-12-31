using opm_validation_service.Models;
using opm_validation_service.Services;

namespace opm_validation_service.Persistence
{
    public interface IUserLimitRepository {
        bool TryAdd(IUser user, int count, out int currentLimit, out int currentBalance);

        bool TryGetUserLimit(IUser user, out int limit);
       
        // Adds new record or resets an old one
        bool AddOrReset(IUser user);
        bool TryRemoveOpm(IUser code);
    }
}