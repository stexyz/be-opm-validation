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

        public bool TryAdd(Opm opm)
        {
            throw new System.NotImplementedException();
        }

        public bool TryRemoveOpm(EanEicCode code)
        {
            throw new System.NotImplementedException();
        }
    }
}