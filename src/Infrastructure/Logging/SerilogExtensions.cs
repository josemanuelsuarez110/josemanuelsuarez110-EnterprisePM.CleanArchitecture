using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ProjectManagementERP.Infrastructure.Logging
{
    public static class SerilogExtensions
    {
        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithEnvironmentName()
                    .WriteTo.Console();
            });

            return hostBuilder;
        }
    }
}
