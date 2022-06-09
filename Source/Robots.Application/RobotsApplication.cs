using Robots.Application.CLI;
using Robots.Application.Extensions;
using Robots.Application.Models;
using Robots.Application.IO;
using Robots.Application.Services;
using Robots.Core.Enums;
using Robots.Core.Interfaces;
using Robots.Core.Models;

namespace Robots.Application
{
    public class RobotsApplication
    {
        private ApplicationInputData _applicationInput = null!;

        private readonly IMarsSurface _marsSurface;
        private readonly IRobotFactory _robotFactory;
        private readonly IConsoleWriter _consoleWriter;
        private readonly IApplicationInputDataParser _applicationInputDataParser;

        private readonly Dictionary<IRobot, IList<IRobotInstruction>> _robotInstructions;

        public RobotsApplication(
            IMarsSurface marsSurface,
            IRobotFactory robotFactory,
            IConsoleWriter consoleWriter,
            IApplicationInputDataParser applicationInputDataParser)
        {
            _marsSurface = marsSurface;
            _robotFactory = robotFactory;
            _consoleWriter = consoleWriter;
            _applicationInputDataParser = applicationInputDataParser;

            _robotInstructions = new Dictionary<IRobot, IList<IRobotInstruction>>();
        }

        public async Task RunAsync(RobotsApplicationArguments args)
        {
            _applicationInput = await _applicationInputDataParser.FromFileAsync(args.InputFilePath);

            _marsSurface.Initialize(new MarsSurfaceInitializeData(
                _applicationInput.SurfaceData.Width, 
                _applicationInput.SurfaceData.Height));

            foreach (var robotInputData in _applicationInput.RobotInputDatas)
            {
                var robot = _robotFactory.FromRobotInputData(robotInputData);
                _robotInstructions.Add(robot, robotInputData.Instructions);

                _marsSurface.AddRobot(robot);
            }

            foreach (var kvp in _robotInstructions)
            {
                var robot = kvp.Key;
                var instructions = kvp.Value;

                foreach (var instruction in instructions)
                {
                    instruction.ExecuteOn(robot);

                    if (robot.State ==  RobotState.Lost)
                    {
                        break;
                    }
                }
            }

            // Not using StringBuilder here because it seems like an overkill
            foreach (var robot in _marsSurface.Robots)
            {
                var output = $"{robot.X} {robot.Y} {robot.Orientation.ToReadable()}";

                if (robot.State == RobotState.Lost)
                {
                    output += " LOST";
                }

                _consoleWriter.WriteLine(output);
            }
        }
    }
}
