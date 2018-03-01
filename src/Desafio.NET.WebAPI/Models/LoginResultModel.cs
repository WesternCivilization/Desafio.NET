using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio.NET.WebAPI.Models
{
    /// <summary>
    /// Access token
    /// </summary>
    public class LoginResultModel
    {
        /// <summary>
        /// Token code
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}