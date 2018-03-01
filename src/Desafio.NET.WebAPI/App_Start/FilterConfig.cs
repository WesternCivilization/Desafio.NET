using Desafio.NET.WebAPI.Filters;
using System.Web;
using System.Web.Http.Filters;

namespace Desafio.NET.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new JWTAuthenticationFilter());
            filters.Add(new ValidateModelFilter());
            filters.Add(new GenericExceptionFilter());
        }
    }
}