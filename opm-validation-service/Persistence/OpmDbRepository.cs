using System;
using System.Globalization;
using System.Linq;
using opm_validation_service.Models;
using opm_validation_service.Persistence.ORM;

//TODO SP: implement
namespace opm_validation_service.Persistence {
    //TODO SP: thread-safe!!!
    public class OpmDbRepository : IOpmRepository
    {
        readonly BE_Opm _dbContext = new BE_Opm();

        public bool TryGetOpm(EanEicCode code, out Opm opmForCode)
        {
            tbl_duplicate_opms dbOpm = _dbContext.tbl_duplicate_opms.SingleOrDefault(o => o.tdo_ean.Equals(code.Code));
            if (dbOpm != null)
            {
                opmForCode = new Opm(new EanEicCode(dbOpm.tdo_ean));
                return true;
            }
            opmForCode = null;
            return false;
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
                if (opm == null)
                {
                    return false;
                }
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
            catch(Exception)
            {
                return false;
            }
        }

        public bool TryRemoveOpm(EanEicCode code)
        {
            throw new System.NotImplementedException();
        }
    }
}