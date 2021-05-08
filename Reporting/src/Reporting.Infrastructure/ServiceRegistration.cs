namespace Reporting.Infrastructure
{
    using Reporting.Infrastructure.Contexts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sieve.Services;

    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // DbContext -- Do Not Delete
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ReportingDbContext>(options =>
                    options.UseInMemoryDatabase($"ReportingDbContext"));
            }
            else
            {
                services.AddDbContext<ReportingDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("ReportingDbContext"),
                        builder => builder.MigrationsAssembly(typeof(ReportingDbContext).Assembly.FullName)));
            }

            services.AddScoped<SieveProcessor>();

            // Auth -- Do Not Delete
        }
    }
}
