using System;
using System.Text;
using Newtonsoft.Json;

namespace business_opportunity.Models
{
    public class OpportunityDetail :  IEquatable<OpportunityDetail>
    {
        public OpportunityDetail(long? detailId = default(long?), string textValue = default(string), DateTime? dttmValue = default(DateTime?), long? intValue = default(long?))
        {
            DetailId = detailId;
            TextValue = textValue;
            DttmValue = dttmValue;
            IntValue = intValue;
            
        }

        /// <summary>
        /// Unique identifier of an opportunity detail
        /// </summary>
        public long? DetailId { get; }

        /// <summary>
        /// Detail value - any text or string
        /// </summary>

        public string TextValue { get; }

        /// <summary>
        /// Detail value of datetime in format 2017-08-23T18:25:43.511, it should conform to ISO 8601 - can be without timezone 
        /// </summary>
        public DateTime? DttmValue { get; }

        /// <summary>
        /// Detail value of integer
        /// </summary>
        public long? IntValue { get; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("OpportunityDetail {\n");
            sb.Append("  DetailId: ").Append(DetailId).Append("\n");
            sb.Append("  TextValue: ").Append(TextValue).Append("\n");
            sb.Append("  DttmValue: ").Append(DttmValue).Append("\n");
            sb.Append("  IntValue: ").Append(IntValue).Append("\n");
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
            return Equals((OpportunityDetail)obj);
        }

        public bool Equals(OpportunityDetail other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    DetailId == other.DetailId ||
                    DetailId != null &&
                    DetailId.Equals(other.DetailId)
                ) && 
                (
                    TextValue == other.TextValue ||
                    TextValue != null &&
                    TextValue.Equals(other.TextValue)
                ) && 
                (
                    DttmValue == other.DttmValue ||
                    DttmValue != null &&
                    DttmValue.Equals(other.DttmValue)
                ) && 
                (
                    IntValue == other.IntValue ||
                    IntValue != null &&
                    IntValue.Equals(other.IntValue)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 41;
                    if (DetailId != null)
                    hash = hash * 59 + DetailId.GetHashCode();
                    if (TextValue != null)
                    hash = hash * 59 + TextValue.GetHashCode();
                    if (DttmValue != null)
                    hash = hash * 59 + DttmValue.GetHashCode();
                    if (IntValue != null)
                    hash = hash * 59 + IntValue.GetHashCode();
                return hash;
            }
        }
        public static bool operator ==(OpportunityDetail left, OpportunityDetail right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OpportunityDetail left, OpportunityDetail right)
        {
            return !Equals(left, right);
        }
    }
}
