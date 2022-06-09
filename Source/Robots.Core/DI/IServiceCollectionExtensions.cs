using Microsoft.Extensions.DependencyInjection;
using Robots.Core.Models;
using FluentValidation;
using Robots.Core.Validators;

namespace Robots.Core.DI
{
    public static class IServiceCollectionExtentions
    {
        public static IServiceCollection AddRobotsCore(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<MarsSurfaceInitializeDataValidator>();

            services.AddSingleton<IMarsSurface, MarsSurface>();

            services.AddTransient<IRobot, Robot>();

            return services;
        }
    }
}
