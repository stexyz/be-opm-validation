﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace opm_validation_service.Persistence
{
    public class UserAccessInMemoryRepository: IUserAccessRepository{
        private readonly Dictionary<string, List<IUserAccessRecord>> store = new Dictionary<string ,List<IUserAccessRecord>>();

        public int GetUserAccessCount(string username, TimeSpan timeWindow)
        {
            if (username == null) {
                throw new ArgumentException("User cannot be null.");
            }

            if (!store.ContainsKey(username))
            {
                return 0;
            }
            int result = store[username].Count(r => r.AccessTime > DateTime.Now.Subtract(timeWindow));
            return result;
        }

        public void RecordAccess(string username, string code, string result)
        {
            if (code == null) {
                throw new ArgumentException("Code cannot be null.");
            }
            if (username == null)
            {
                username = "";
            }

            if (!store.ContainsKey(username))
            {
                store[username] = new List<IUserAccessRecord>();
            }
            store[username].Add(new UserAccessRecord(username, DateTime.Now, code, result));
        }
    }
}