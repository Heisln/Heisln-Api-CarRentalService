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
    public partial class Booking
    {
        /// <summary>
        /// Gets or Sets CarId
        /// </summary>
        [DataMember(Name = "Id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or Sets CarId
        /// </summary>
        [DataMember(Name = "carId")]
        public Guid? CarId { get; set; }

        /// <summary>
        /// Gets or Sets CarId
        /// </summary>
        [DataMember(Name = "car")]
        public CarInfo? Car { get; set; }

        /// <summary>
        /// Gets or Sets CarId
        /// </summary>
        [DataMember(Name = "userId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }
      

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
