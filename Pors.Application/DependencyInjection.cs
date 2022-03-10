﻿using System;
using MediatR;
using Loby.Tools;
using Loby.Extensions;
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

            services.AddAutoMapper(configs =>
            {
                configs.CreateMap<DateTime, string>().ConvertUsing(x => Dater.ToIranSolar(x, "yyyy/MM/dd"));

            }, executingAssembly);

            services.AddMediatR(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);

            return services;
        }
    }
}
