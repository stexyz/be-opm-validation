using System;
using System.Text;
using Newtonsoft.Json;

namespace business_opportunity.Models
{
    public class Error :  IEquatable<Error>
    {
        public Error(int? code = default(int?), string message = default(string))
        {
            Code = code;
            Message = message;
            
        }

        public int? Code { get; }

        public string Message { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Error {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
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
            return Equals((Error)obj);
        }

        public bool Equals(Error other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Code == other.Code ||
                    Code != null &&
                    Code.Equals(other.Code)
                ) && 
                (
                    Message == other.Message ||
                    Message != null &&
                    Message.Equals(other.Message)
                );
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 41;
                    if (Code != null)
                    hash = hash * 59 + Code.GetHashCode();
                    if (Message != null)
                    hash = hash * 59 + Message.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Error left, Error right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Error left, Error right)
        {
            return !Equals(left, right);
        }
    }
}
