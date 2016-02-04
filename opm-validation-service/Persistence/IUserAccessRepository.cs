using System;

namespace opm_validation_service.Persistence
{
    public interface IUserAccessRepository {
        int GetUserAccessCount(string username, TimeSpan timeWindow);

        //TODO SP: remove old access logs 
        void RecordAccess(string username, string code, string result);
    }
}