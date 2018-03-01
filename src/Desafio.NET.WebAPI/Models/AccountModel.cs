using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Desafio.NET.WebAPI.Models
{
    /// <summary>
    /// Account
    /// </summary>
    public class AccountModel
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Name of user
        /// </summary>
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// E-mail address
        /// </summary>
        [JsonProperty("email")]
        [Required]
        [RegularExpression("^[0-9a-zA-Z]+([0-9a-zA-Z]*[-._+])*[0-9a-zA-Z]+@[0-9a-zA-Z]+([-.][0-9a-zA-Z]+)*([0-9a-zA-Z]*[.])[a-zA-Z]{2,6}$", ErrorMessage = "Invalid e-mail!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// User phones
        /// </summary>
        [JsonProperty("phones")]
        public AccountPhoneModel[] Phones { get; set; }
    }
}