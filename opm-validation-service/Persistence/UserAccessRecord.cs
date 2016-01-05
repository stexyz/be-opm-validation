using System;
using opm_validation_service.Models;

namespace opm_validation_service.Persistence
{
    public class UserAccessRecord : IUserAccessRecord
    {
        public UserAccessRecord(string userId, DateTime accessTime, EanEicCode code)
        {
            UserId = userId;
            AccessTime = accessTime;
            Code = code;
        }
        public string UserId { get; private set; }
        public DateTime AccessTime { get; private set; }
        public EanEicCode Code { get; private set; }
    }
}