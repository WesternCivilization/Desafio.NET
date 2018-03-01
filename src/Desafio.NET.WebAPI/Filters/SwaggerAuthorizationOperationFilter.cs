using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;

namespace Desafio.NET.WebAPI.Filters
{
    /// <summary>
    /// Permite a parametrização da autenticação usando cabeçalho
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerAuthorizationOperationFilter : IOperationFilter
    {
        ///// <summary>
        ///// Adiciona o parâmetro de autorização no cabeçalho
        ///// </summary>
        ///// <param name="operation"></param>
        ///// <param name="context"></param>
        //public void Apply(Operation operation, OperationFilterContext context)
        //{
        //    var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
        //    var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
        //    var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

        //    if (isAuthorized && !allowAnonymous)
        //    {
        //        if (operation.Parameters == null)
        //            operation.Parameters = new List<IParameter>();
        //        operation.Parameters.Add(new NonBodyParameter
        //        {
        //            Name = "Authorization",
        //            In = "header",
        //            Description = "Bearer token",
        //            Required = true,
        //            Type = "string"
        //        });
        //    }
        //}

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Instance).Any(filter => filter is IAuthorizationFilter);
            var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>();

                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "Bearer token",
                    required = true,
                    type = "string"
                });
            }
        }
    }
}