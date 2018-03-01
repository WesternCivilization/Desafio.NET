using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio.NET.WebAPI.Models
{
    public class NonSuccessValidationResponseModel: NonSuccessResponseModel
    {
        public Dictionary<string, string> Errors { get; set; }
    }
}