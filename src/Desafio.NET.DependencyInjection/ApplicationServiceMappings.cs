using Autofac;
using Desafio.NET.ApplicationServices;
using Desafio.NET.ApplicationServices.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.DependencyInjection
{
    public class ApplicationServiceMappings : Module
    {
        /// <summary>
        /// Load mappings
        /// </summary>
        /// <param name="builder">Container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountApplicationService>().As<IAccountApplicationService>();
            builder.RegisterType<LoginApplicationService>().As<ILoginApplicationService>();
        }
    }
}