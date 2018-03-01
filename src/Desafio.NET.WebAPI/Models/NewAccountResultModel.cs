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
    public class NewAccountResultModel: NewAccountModel
    {
        /// <summary>
        /// Token code
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}