using System;
using System.Linq;

namespace Robots.Application.Tests.Services
{
    public class RobotFactoryTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly RobotFactory _SUT;

        public RobotFactoryTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Default);
            _SUT = new RobotFactory(_serviceProviderMock.Object);
        }

        [Theory]
        [InlineData(1, 1, Orientation.East)]
        [InlineData(2, 1, Orientation.South)]
        [InlineData(3, 3, Orientation.North)]
        [InlineData(8, 6, Orientation.West)]
        [InlineData(2, 4, Orientation.Unknown)]
        public void FromRobotInputData_Passes_All_Information_To_Robot_Initialize(
            int x, int y, Orientation orientation)
        {
            // Arrage
            var robotMock = new Mock<IRobot>();

            _serviceProviderMock
                .Setup(a => a.GetService(typeof(IRobot)))
                .Returns(robotMock.Object);

            // Act
            _SUT.FromRobotInputData(new(x, y, orientation, 
                Enumerable.Empty<IRobotInstruction>().ToList()));

            // Assert
            robotMock.Verify(a => a.Initialize(new RobotInitializeData(x, y, orientation)), Times.Once);
        }
    }
}
