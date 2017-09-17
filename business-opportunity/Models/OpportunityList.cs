using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace business_opportunity.Models
{
    public class OpportunityList :  IEquatable<OpportunityList>
    {
        public OpportunityList(List<OpportunityListItem> opportunities = default(List<OpportunityListItem>), int? count = default(int?))
        {
            Opportunities = opportunities;
            Count = count;
        }

        public List<OpportunityListItem> Opportunities { get; }
        /// <summary>
        /// The total number of all opportunities for given query without pagination
        /// </summary>
        public int? Count { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("OpportunityList {\n");
            sb.Append("  Opportunities: ").Append(Opportunities).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
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
            return Equals((OpportunityList)obj);
        }

        public bool Equals(OpportunityList other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Opportunities == other.Opportunities ||
                    Opportunities != null &&
                    Opportunities.SequenceEqual(other.Opportunities)
                ) && 
                (
                    Count == other.Count ||
                    Count != null &&
                    Count.Equals(other.Count)
                );
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 41;
                    if (Opportunities != null)
                    hash = hash * 59 + Opportunities.GetHashCode();
                    if (Count != null)
                    hash = hash * 59 + Count.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(OpportunityList left, OpportunityList right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OpportunityList left, OpportunityList right)
        {
            return !Equals(left, right);
        }
    }
}
