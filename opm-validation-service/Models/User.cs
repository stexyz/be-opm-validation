namespace opm_validation_service.Models
{
    public class User : IUser{
        public User(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}