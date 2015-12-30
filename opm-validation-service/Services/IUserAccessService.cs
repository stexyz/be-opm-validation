namespace opm_validation_service.Services {
    public interface IUserAccessService
    {
        /// <summary>
        /// Synchronized method. Tries to access resource. If successful, the internal counter gets increased.
        /// This service also maintains access logs.
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True if there is some limit left. False otherwise.</returns>
        bool TryAccess(IUser user);
    }
}