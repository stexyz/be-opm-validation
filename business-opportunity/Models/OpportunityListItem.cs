using System;
using System.Text;
using Newtonsoft.Json;

namespace business_opportunity.Models
{
    public class OpportunityListItem :  IEquatable<OpportunityListItem>
    {

        public OpportunityListItem(long? id = default(long?), string status = default(string), string type = default(string))
        {
            Id = id;
            Status = status;
            Type = type;
            
        }

        /// <summary>
        /// Unique identifier of an opportunity
        /// </summary>
        public long? Id { get; }

        /// <summary>
        /// Status of an opportunity
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Type of an opportunity
        /// </summary>
        public string Type { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("OpportunityListItem {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OpportunityListItem)obj);
        }

        public bool Equals(OpportunityListItem other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
                ) && 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
                );
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 41;
                    if (Id != null)
                    hash = hash * 59 + Id.GetHashCode();
                    if (Status != null)
                    hash = hash * 59 + Status.GetHashCode();
                    if (Type != null)
                    hash = hash * 59 + Type.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(OpportunityListItem left, OpportunityListItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OpportunityListItem left, OpportunityListItem right)
        {
            return !Equals(left, right);
        }
    }
}
