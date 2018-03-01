using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio.NET.WebAPI.Models
{
    /// <summary>
    /// Account phone
    /// </summary>
    public class AccountPhoneModel
    {
        /// <summary>
        /// Area code
        /// </summary>
        [JsonProperty("areaCode")]
        public string AreaCode { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}