using opm_validation_service.Models;

namespace opm_validation_service.Persistence {
    public interface IOpmRepository {
        bool TryGetOpm(EanEicCode code, out Opm opmForCode);
        bool TryAdd(Opm opm);
        bool TryRemoveOpm(EanEicCode code);
    }
}
