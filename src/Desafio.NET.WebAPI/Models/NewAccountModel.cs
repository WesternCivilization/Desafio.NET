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
    public class NewAccountModel: AccountModel
    {
        /// <summary>
        /// Name of user
        /// </summary>
        [JsonProperty("password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}