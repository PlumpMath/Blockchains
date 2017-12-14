using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace FluentCodeAPI.AspNetCore.Blockchains
{
    /// <summary>
    /// Extension methods for setting up the blockain service in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class FluentCodeApiServiceCollectionExtensions
    {
        /// <summary>
        /// Adds blockain service to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the services.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <see cref="IServiceCollection" /> is null.</exception>
        public static IServiceCollection AddBlockchain(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<Blockchain>();

            return services;
        }
    }
}