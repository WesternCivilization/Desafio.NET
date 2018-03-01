using AutoMapper;
using Desafio.NET.Domain.Entities;
using Desafio.NET.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio.NET.WebAPI.Mappings
{
    public class LoginProfile: Profile
    {
        public LoginProfile()
        {
            CreateMap<Token, LoginResultModel>().ForMember(destination => destination.AccessToken, source => source.MapFrom(field => field.Code));
        }
    }
}