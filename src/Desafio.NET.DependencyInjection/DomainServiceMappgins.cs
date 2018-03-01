using Autofac;
using Desafio.NET.Domain.Services;
using Desafio.NET.Domain.Services.Abstractions;
using Desafio.NET.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.DependencyInjection
{
    public class DomainServiceMappgins : Module
    {
        /// <summary>
        /// Load mappings
        /// </summary>
        /// <param name="builder">Container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ITokenDomainService>((context, repository) =>
            {
                return new TokenDomainService(context.Resolve<ITokenRepository>(), context.Resolve<IUserRepository>(), ConfigurationManager.AppSettings.Get("Jwt:SecretKey"));
            });

            builder.RegisterType<UserDomainService>().As<IUserDomainService>();
        }
    }
}