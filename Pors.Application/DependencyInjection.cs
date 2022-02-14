using System;
using MediatR;
using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(executingAssembly);
            services.AddAutoMapper(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);

            return services;
        }
    }
}
