using AutoMapper;
using Desafio.NET.Domain.Entities;
using Desafio.NET.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio.NET.WebAPI.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<User, AccountModel>();
            CreateMap<User, NewAccountResultModel>().ForMember(destination => destination.AccessToken, source => source.MapFrom(field => field.AccessToken.Code));
            CreateMap<UserPhone, AccountPhoneModel>();

            CreateMap<NewAccountModel, User>();
            CreateMap<AccountPhoneModel, UserPhone>();
        }
    }
}