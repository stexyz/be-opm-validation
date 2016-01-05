using System;
using System.Collections.Generic;
using System.Linq;
using opm_validation_service.Models;

namespace opm_validation_service.Persistence
{
    public class UserAccessInMemoryRepository: IUserAccessRepository{
        private readonly Dictionary<string, List<IUserAccessRecord>> store = new Dictionary<string ,List<IUserAccessRecord>>();

        public int GetUserAccessCount(IUser user, TimeSpan timeWindow)
        {
            if (!store.ContainsKey(user.Id))
            {
                return 0;
            }
            int result = store[user.Id].Count(r => r.AccessTime > DateTime.Now.Subtract(timeWindow));
            return result;
        }

        public void RecordAccess(IUser user, EanEicCode code)
        {
            if (!store.ContainsKey(user.Id))
            {
                store[user.Id] = new List<IUserAccessRecord>();
            }
            store[user.Id].Add(new UserAccessRecord(user.Id, DateTime.Now, code));
        }
    }
}