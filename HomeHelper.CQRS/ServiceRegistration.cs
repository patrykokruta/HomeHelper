using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HomeHelper.CQRS
{
    public static class ServiceRegistration
    {
        public static void AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
