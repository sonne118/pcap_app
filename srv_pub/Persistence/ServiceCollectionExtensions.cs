﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace srv_pub.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            services.AddApplicationDbContext<TContext>(connectionString);
            services.TryAddScoped<DbContext>(sp => sp.GetRequiredService<TContext>());
            return services;
        }

        private static void AddApplicationDbContext<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>((sp, options) =>
            {
                options
                    .UseMySql(
                        connectionString,
                        new MySqlServerVersion(new Version(9, 1, 0)), // Use your MySQL server version
                        cfg => cfg.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            });
        }
    }
}
