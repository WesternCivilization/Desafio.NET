using Autofac;
using Desafio.NET.Repository;
using Desafio.NET.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.DependencyInjection
{
    public class RepositoryMappings : Module
    {
        /// <summary>
        /// Load mappings
        /// </summary>
        /// <param name="builder">Container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IUserRepository>((context, repository) =>
            {
                return new UserRepository(ConfigurationManager.ConnectionStrings["Desafio.NET"].ConnectionString);
            });

            builder.Register<IUserPhoneRepository>((context, repository) =>
            {
                return new UserPhoneRepository(ConfigurationManager.ConnectionStrings["Desafio.NET"].ConnectionString);
            });

            builder.Register<ITokenRepository>((context, repository) =>
            {
                return new TokenRepository(ConfigurationManager.ConnectionStrings["Desafio.NET"].ConnectionString);
            });
        }
    }
}
