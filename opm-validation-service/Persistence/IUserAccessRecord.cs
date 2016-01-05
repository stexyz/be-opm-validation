using System;
using opm_validation_service.Models;

namespace opm_validation_service.Persistence
{
    internal interface IUserAccessRecord
    {
        string UserId { get; }
        DateTime AccessTime { get;  }
        EanEicCode Code { get; }
    }
}