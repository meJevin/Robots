using Robots.Application.Exceptions;
using Robots.Application.Extensions;
using Robots.Application.IO;
using Robots.Application.Models;
using Robots.Core.Interfaces;

namespace Robots.Application.Services
{
    public interface IApplicationInputDataParser
    {
        Task<ApplicationInputData> FromFileAsync(string filePath);
        Task<ApplicationInputData> FromStringAsync(string input);
    }

    public class ApplicationInputDataParser : IApplicationInputDataParser
    {
        private readonly IRobotInstructionFactory _robotInstructionFactory;
        private readonly IFileReader _fileReader;

        public ApplicationInputDataParser(
            IRobotInstructionFactory robotInstructionFactory, 
            IFileReader fileReader)
        {
            _robotInstructionFactory = robotInstructionFactory;
            _fileReader = fileReader;
        }

        public async Task<ApplicationInputData> FromFileAsync(string filePath)
        {
            // Abstracting file access would be overkill, come on
            var fileText = await _fileReader.ReadAllTextAsync(filePath);

            return await FromStringAsync(fileText);
        }

        public virtual Task<ApplicationInputData> FromStringAsync(string input)
        {
            var inputLines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim(' ', '\r', '\n')).ToList();

            if (inputLines.Count < 1)
            {
                throw new ApplicationInputDataParserException("Mars surface data not provided");
            }

            var marsSurfaceLine = inputLines[0];
            var marsSurfaceInputData = ParseMarsSurfaceInputData(marsSurfaceLine);

            // We assume that we may not have any robots at all ;)

            var robotInputDatas = new List<RobotInputData>();
            for (int i = 1; i < inputLines.Count; i += 2)
            {
                if ((i + 1) >= inputLines.Count) break;

                var posLine = inputLines[i];
                var instructionsLine = inputLines[i + 1];

                var robotInputData = ParseRobotInputData(posLine, instructionsLine);
                robotInputDatas.Add(robotInputData);
            }

            return Task.FromResult(new ApplicationInputData(marsSurfaceInputData, robotInputDatas));
        }

        // Public virtual for easier unit tests
        public virtual MarsSurfaceInputData ParseMarsSurfaceInputData(string marsSurfaceLine)
        {
            var tokens = marsSurfaceLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
            {
                throw new ApplicationInputDataParserException("Incorrect format of mars surface data");
            }

            var widthIsParsed = int.TryParse(tokens[0], out var width);
            var heightIsParsed = int.TryParse(tokens[1], out var height);

            if (!widthIsParsed)
            {
                throw new ApplicationInputDataParserException(
                    $"Could not parse width value for mars surface. Provided token: {tokens[0]}");
            }

            if (!heightIsParsed)
            {
                throw new ApplicationInputDataParserException(
                    $"Could not parse height value for mars surface. Provided token: {tokens[1]}");
            }

            return new MarsSurfaceInputData(width, height);
        }

        // Public virtual for easier unit tests
        public virtual RobotInputData ParseRobotInputData(string posLine, string instructionsLine)
        {
            var tokensPos = posLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokensPos.Length != 3)
            {
                throw new ApplicationInputDataParserException($"Incorrect format of robot data position data. Got {posLine}");
            }

            var XIsParsed = int.TryParse(tokensPos[0], out var x);
            var YIsParsed = int.TryParse(tokensPos[1], out var y);

            if (!XIsParsed)
            {
                throw new ApplicationInputDataParserException(
                    $"Could not parse position X value for robot. Got: {posLine}");
            }

            if (!YIsParsed)
            {
                throw new ApplicationInputDataParserException(
                    $"Could not parse position Y value for robot. Got: {posLine}");
            }

            var orientationParsed = OrientationExtensions.TryParse(tokensPos[2], out var orientation);

            if (!orientationParsed)
            {
                throw new ApplicationInputDataParserException(
                    $"Could not parse orientation value for robot. Got: {posLine}");
            }

            var instructions = new List<IRobotInstruction>();

            for (int i = 0; i < instructionsLine.Length; ++i)
            {
                var name = instructionsLine[i].ToString();
                instructions.Add(_robotInstructionFactory.FromName(name));
            }

            return new RobotInputData(x, y, orientation, instructions);
        }
    }
}
