using CommandLine;

namespace Robots.Application.CLI
{
    public class RobotsApplicationArguments
    {
        [Option('f', "file", Required = true, HelpText = "Please enter the input file name")]
        public string InputFilePath { get; init; }
    }
}
