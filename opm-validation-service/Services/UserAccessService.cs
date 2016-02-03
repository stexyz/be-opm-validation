using System;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Services
{
    public class UserAccessService : IUserAccessService
    {
        public UserAccessService(IUserAccessRepository userAccessRepository, TimeSpan timeWindow, int maxAccessCount)
        {
            _userAccessRepository = userAccessRepository;
            _timeWindow = timeWindow;
            _maxAccessCount = maxAccessCount;
        }
        
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly TimeSpan _timeWindow;
        private readonly int _maxAccessCount;

        public bool TryAccess(string username, EanEicCode code)
        {
            int accessCount = _userAccessRepository.GetUserAccessCount(username, _timeWindow);
            _userAccessRepository.RecordAccess(username, code);

            return accessCount < _maxAccessCount;
        }
    }
}