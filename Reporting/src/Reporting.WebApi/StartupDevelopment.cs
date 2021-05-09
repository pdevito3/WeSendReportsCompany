namespace Reporting.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Reporting.Infrastructure;
    using Reporting.Infrastructure.Seeders;
    using Reporting.Infrastructure.Contexts;
    using Reporting.WebApi.Extensions;
    using Serilog;
    using MassTransit;
    using Messages;
    using RabbitMQ.Client;

    public class StartupDevelopment
    {
        public IConfiguration _config { get; }
        public IWebHostEnvironment _env { get; }

        public StartupDevelopment(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsService("ReportingCorsPolicy");
            services.AddInfrastructure(_config, _env);

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.Message<ISendReportRequest>(e => e.SetEntityName("send-report")); // name of the primary exchange
                    cfg.Publish<ISendReportRequest>(e => e.ExchangeType = ExchangeType.Topic); // primary exchange type
                    cfg.Send<ISendReportRequest>(e =>
                    {
                        e.UseRoutingKeyFormatter(context =>
                        {
                            var sharedState = context.Message.IsPublic ? "public" : "private";
                            return $"{sharedState}.{context.Message.Provider}";
                        });
                    });
                });
            });
            services.AddMassTransitHostedService();

            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddWebApiServices();
            services.AddHealthChecks();

            // Dynamic Services
            services.AddSwaggerExtension(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            // Entity Context - Do Not Delete

            using (var context = app.ApplicationServices.GetService<ReportingDbContext>())
            {
                context.Database.EnsureCreated();

                // ReportingDbContext Seeders
                ReportRequestSeeder.SeedSampleReportRequestData(app.ApplicationServices.GetService<ReportingDbContext>());
            }

            app.UseCors("ReportingCorsPolicy");

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });

            // Dynamic App
            app.UseSwaggerExtension(_config);
        }
    }
}