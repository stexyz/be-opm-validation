using System.Linq;
using Common.Logging;
using opm_validation_service.Models;
using opm_validation_service.Persistence.ORM;

namespace opm_validation_service.Persistence {
    public class OpmDbRepository : IOpmRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (OpmDbRepository));
        
        readonly BE_Opm _dbContext = new BE_Opm();

        public bool TryGetOpm(EanEicCode code, out Opm opmForCode)
        {
            try
            {
                _log.Debug(m => m("Trying to fetch Opm code=[" + " code.ToString() " + "]."));
                tbl_duplicate_opms dbOpm = _dbContext.tbl_duplicate_opms.SingleOrDefault(o => o.tdo_ean.Equals(code.Code));
                opmForCode = new Opm(new EanEicCode(dbOpm.tdo_ean));
                _log.Debug("Opm code=[" + " code.ToString() " + "] found in repository.");
                return true;
            }
            catch
            {
                opmForCode = null;
                _log.Error("Opm could not be fetched from repository. Exception thrown.");
                return false;
            }
        }

        /// <summary>
        /// Tries to add a new opm record to DB.
        /// </summary>
        /// <param name="opm">Opm to add.</param>
        /// <returns>True if addition had suceeded.</returns>
        public bool TryAdd(Opm opm)
        {
            try
            {
                _log.Debug("Addition of Opm code=[" + " code.ToString() " + "] started.");
                tbl_duplicate_opms newRecord = new tbl_duplicate_opms {
                        tdo_cp_id = 0,
                        tdo_ean = opm.Code.Code,
                        tdo_is_opm_duplicate = true
                    };

                _dbContext.tbl_duplicate_opms.Add(newRecord);
                _dbContext.SaveChanges();
                _log.Debug("Addition of Opm code=[" + " code.ToString() " + "] succeeded.");
                return true;
            }
            catch
            {
                _log.Error("Addition of Opm failed with an exception.");
                return false;
            }
        }

        public bool TryRemoveOpm(EanEicCode code)
        {
            try {
                _log.Debug("Removal of Opm code=[" + " code.ToString() " + "] started.");
                tbl_duplicate_opms entityToRemove = _dbContext.tbl_duplicate_opms.FirstOrDefault(opm => opm.tdo_ean.Equals(code.Code));
                if (entityToRemove == null)
                {
                    return false;
                }
                _dbContext.tbl_duplicate_opms.Remove(entityToRemove);
                _dbContext.SaveChanges();
                _log.Debug("Removal of Opm code=[" + " code.ToString() " + "] finished.");
                return true;
            } catch {
                _log.Error("Removal of Opm failed with an exception.");
                return false;
            }
        }
    }
}