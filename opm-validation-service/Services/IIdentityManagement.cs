namespace opm_validation_service.Services
{
    public interface IIdentityManagement {
        /// <summary>
        /// Queries IDM (OpenSSO) to find out if the token is not expired (and known to the system).
        /// </summary>
        /// <param name="token">Token to validate.</param>
        /// <returns>True if token is valid and not expired.</returns>
        bool ValidateUser(string token);

        /// <summary>
        /// Queries IDM (OpenSSO) to retrieve information about user from the given token.
        /// </summary>
        /// <param name="token">Token to get info for.</param>
        /// <returns>Info about user.</returns>
        IUser GetUserInfo(string token);

        /// <summary>
        /// Acquires token from IDM (OpenSSO).
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="password">password</param>
        /// <returns>Token.</returns>
        string Login(string userName, string password);
    }
}