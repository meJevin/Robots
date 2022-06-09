using Microsoft.Extensions.DependencyInjection;
using Robots.Application.Models;
using Robots.Core.Models;

namespace Robots.Application.Services
{
    public interface IRobotFactory
    {
        IRobot FromRobotInputData(RobotInputData inputData);
    }

    public class RobotFactory : IRobotFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RobotFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRobot FromRobotInputData(RobotInputData inputData)
        {
            var robot = _serviceProvider.GetRequiredService<IRobot>();
            robot.Initialize(new(inputData.X, inputData.Y, inputData.Orientation));

            return robot;
        }
    }
}
