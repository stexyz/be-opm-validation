using System;
using System.Linq;
using opm_validation_service.Models;
using opm_validation_service.Persistence.ORM;

namespace opm_validation_service.Persistence
{
    /// <summary>
    /// Repo for user access limits
    /// </summary>
    public class UserAccessDbRepository : IUserAccessRepository {
        readonly BE_Opm _dbContext = new BE_Opm();
        /// <summary>
        /// Returns number of access records in the specified time window (from now).
        /// </summary>
        /// <param name="username">User to get records for.</param>
        /// <param name="timeWindow">Timespan until when to count records.</param>
        /// <returns>Count of valid records.</returns>
        public int GetUserAccessCount(string username, TimeSpan timeWindow)
        {
            if (username == null) {
                throw new ArgumentException("User cannot be null.");
            }
            DateTime beginningOfTimeWindow = DateTime.UtcNow.Subtract(timeWindow);
            IQueryable<tbl_user_access_log> dbUserAccessRecords = _dbContext.tbl_user_access_log.Where(l => l.tdo_user_id.Equals(username) &&
                                                                                                            l.tdo_access_time > beginningOfTimeWindow);
            return dbUserAccessRecords.Count();
        }

        public void RecordAccess(string username, EanEicCode code)
        {
            if (username == null) {
                throw new ArgumentException("User cannot be null.");
            }
            if (code == null) {
                throw new ArgumentException("Code cannot be null.");
            }

            tbl_user_access_log newRecord = new tbl_user_access_log
                {
                    tdo_user_id = username,
                    tdo_ean = code.Code,
                    tdo_access_time = DateTime.UtcNow
                };

            _dbContext.tbl_user_access_log.Add(newRecord);
            _dbContext.SaveChanges();
        }
    }
}