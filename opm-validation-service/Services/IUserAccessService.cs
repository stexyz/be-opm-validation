using opm_validation_service.Models;

namespace opm_validation_service.Services {
    public interface IUserAccessService
    {
        /// <summary>
        /// Synchronized method. Tries to access resource. If successful, the internal counter gets increased.
        /// This service also maintains access logs.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="code"></param>
        /// <returns>True if there is some limit left. False otherwise.</returns>
        bool TryAccess(IUser user, EanEicCode code);

        //TODO SP:
        // string GetAccessLogs();
        // string GetAccessLogs(IUser user);
        // void RemoveOldLogs();
    }
}