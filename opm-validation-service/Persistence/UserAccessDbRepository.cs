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
        /// <param name="user">User to get records for.</param>
        /// <param name="timeWindow">Timespan until when to count records.</param>
        /// <returns>Count of valid records.</returns>
        public int GetUserAccessCount(IUser user, TimeSpan timeWindow)
        {
            if (user == null) {
                throw new ArgumentException("User cannot be null.");
            }
            
            IQueryable<tbl_user_access_log> dbUserAccessRecords = _dbContext.tbl_user_access_log.Where(l => l.tdo_user_id.Equals(user.Id));
            return dbUserAccessRecords.Count();
        }

        public void RecordAccess(IUser user, EanEicCode code)
        {
            if (user == null) {
                throw new ArgumentException("User cannot be null.");
            }
            if (code == null) {
                throw new ArgumentException("Code cannot be null.");
            }

            tbl_user_access_log newRecord = new tbl_user_access_log
                {
                    tdo_user_id = user.Id,
                    tdo_ean = code.Code,
                    tdo_access_time = DateTime.UtcNow
                };

            _dbContext.tbl_user_access_log.Add(newRecord);
            _dbContext.SaveChanges();
        }
    }
}