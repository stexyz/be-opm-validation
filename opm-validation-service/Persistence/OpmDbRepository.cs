using System.Linq;
using opm_validation_service.Models;
using opm_validation_service.Persistence.ORM;

namespace opm_validation_service.Persistence {
    public class OpmDbRepository : IOpmRepository
    {
        readonly BE_Opm _dbContext = new BE_Opm();

        public bool TryGetOpm(EanEicCode code, out Opm opmForCode)
        {
            try
            {
                tbl_duplicate_opms dbOpm =
                    _dbContext.tbl_duplicate_opms.SingleOrDefault(o => o.tdo_ean.Equals(code.Code));
                opmForCode = new Opm(new EanEicCode(dbOpm.tdo_ean));
                return true;
            }
            catch
            {
                opmForCode = null;
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
                tbl_duplicate_opms newRecord = new tbl_duplicate_opms
                    {
                        tdo_cp_id = 0,
                        tdo_ean = opm.Code.Code,
                        tdo_is_opm_duplicate = true
                    };

                _dbContext.tbl_duplicate_opms.Add(newRecord);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryRemoveOpm(EanEicCode code)
        {
            try {
                tbl_duplicate_opms entityToRemove = _dbContext.tbl_duplicate_opms.FirstOrDefault(opm => opm.tdo_ean.Equals(code.Code));
                if (entityToRemove == null)
                {
                    return false;
                }
                _dbContext.tbl_duplicate_opms.Remove(entityToRemove);
                _dbContext.SaveChanges();
                return true;
            } catch {
                return false;
            }
        }
    }
}