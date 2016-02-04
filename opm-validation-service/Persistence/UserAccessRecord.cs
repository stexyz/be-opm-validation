using System;

namespace opm_validation_service.Persistence
{
    public class UserAccessRecord : IUserAccessRecord
    {
        public UserAccessRecord(string userId, DateTime accessTime, string code, string result)
        {
            UserId = userId;
            AccessTime = accessTime;
            Code = code;
            Result = result;
        }
        public string UserId { get; private set; }
        public DateTime AccessTime { get; private set; }
        public string Code { get; private set; }
        public string Result { get; private set; }
    }
}