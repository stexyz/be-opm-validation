﻿using System;
using opm_validation_service.Models;

namespace opm_validation_service.Persistence
{
    public interface IUserAccessRepository {
        int GetUserAccessCount(IUser user, TimeSpan timeWindow);
        void RecordAccess(IUser user, EanEicCode code);
        //TODO SP: solve how the old records will be handled - for audit this data has value, however we don't want to introduce a leak here
//        void RemoveOldLogs(TimeSpan timeWindow);
    }
}