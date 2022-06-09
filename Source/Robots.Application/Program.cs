using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Robots.Application.CLI;
using Robots.Application.IO;
using Robots.Application.Services;
using Robots.Core.DI;

namespace Robots.Application
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default
                .ParseArguments<RobotsApplicationArguments>(args)
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        Console.Error.WriteLine(error.ToString());
                    }
                })
                .WithParsedAsync(async parsedArgs =>
                {
                    try
                    {
                        var serviceProvider = BuildServiceProvider();

                        var app = serviceProvider.GetRequiredService<RobotsApplication>();

                        await app.RunAsync(parsedArgs);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while executing program. Message: {ex.Message}");
                    }
                });
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // Robots core DI
            services.AddRobotsCore();

            // The whole application
            services.AddSingleton<RobotsApplication>();

            // Robot factory
            services.AddSingleton<IRobotFactory, RobotFactory>();

            // Robot instruction factory
            services.AddSingleton<IRobotInstructionFactory, RobotInstructionFactory>();

            // Input data parser
            services.AddSingleton<IApplicationInputDataParser, ApplicationInputDataParser>();

            // Console writer
            services.AddSingleton<IConsoleWriter, ConsoleWriter>();

            // File reader
            services.AddSingleton<IFileReader, FileReader>();

            return services.BuildServiceProvider();
        }
    }
}