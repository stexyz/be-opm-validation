namespace opm_validation_service.Models
{
    public class User : IUser{
        protected bool Equals(User other) {
            return string.Equals(Id, other.Id);
        }

        public override int GetHashCode() {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public override bool Equals(object obj) {
            User other = (User)obj;
            if (obj == null || other == null) {
                return false;
            }
            return Equals(other);
        }

        public User(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}