using opm_validation_service.Models;

//TODO SP: implement
namespace opm_validation_service.Persistence {

    public class OpmDbRepository : IOpmRepository
    {
        public bool TryGetOpm(EanEicCode code, out Opm opmForCode)
        {
            throw new System.NotImplementedException();
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