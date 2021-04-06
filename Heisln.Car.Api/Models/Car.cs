using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Car : IEquatable<Car>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Brand
        /// </summary>
        [DataMember(Name = "brand")]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Horsepower
        /// </summary>
        [DataMember(Name = "horsepower")]
        public int? Horsepower { get; set; }

        /// <summary>
        /// Gets or Sets Consumption
        /// </summary>
        [DataMember(Name = "consumption")]
        public double? Consumption { get; set; }

        /// <summary>
        /// Gets or Sets Priceperday
        /// </summary>
        [DataMember(Name = "priceperday")]
        public double? Priceperday { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Car {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Brand: ").Append(Brand).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Horsepower: ").Append(Horsepower).Append("\n");
            sb.Append("  Consumption: ").Append(Consumption).Append("\n");
            sb.Append("  Priceperday: ").Append(Priceperday).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Car)obj);
        }

        /// <summary>
        /// Returns true if Car instances are equal
        /// </summary>
        /// <param name="other">Instance of Car to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Car other)
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
                    Brand == other.Brand ||
                    Brand != null &&
                    Brand.Equals(other.Brand)
                ) &&
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&
                (
                    Horsepower == other.Horsepower ||
                    Horsepower != null &&
                    Horsepower.Equals(other.Horsepower)
                ) &&
                (
                    Consumption == other.Consumption ||
                    Consumption != null &&
                    Consumption.Equals(other.Consumption)
                ) &&
                (
                    Priceperday == other.Priceperday ||
                    Priceperday != null &&
                    Priceperday.Equals(other.Priceperday)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                if (Brand != null)
                    hashCode = hashCode * 59 + Brand.GetHashCode();
                if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                if (Horsepower != null)
                    hashCode = hashCode * 59 + Horsepower.GetHashCode();
                if (Consumption != null)
                    hashCode = hashCode * 59 + Consumption.GetHashCode();
                if (Priceperday != null)
                    hashCode = hashCode * 59 + Priceperday.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Car left, Car right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Car left, Car right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
