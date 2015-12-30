namespace opm_validation_service.Services
{
    public class UserAccessMockService : IUserAccessService
    {
        public bool TryAccess(IUser user)
        {
            return true;
        }
    }
}