using Robots.Application.CLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robots.Application.Tests
{
    public class RobotsApplicationTests
    {
        private readonly RobotsApplication _SUT;

        private readonly Mock<IMarsSurface> _marsSurfaceMock;
        private readonly Mock<IRobotFactory> _robotFactoryMock;
        private readonly Mock<IConsoleWriter> _consoleWriterMock;
        private readonly Mock<IApplicationInputDataParser> _inputParserMock;

        ApplicationInputData _applicationInput = new ApplicationInputData(
            new MarsSurfaceInputData(5, 3),
            new List<RobotInputData>
            {
                    new RobotInputData(1, 1, Orientation.East, new List<IRobotInstruction>
                    {
                        new TurnRightInstruction(),
                        new TurnLeftInstruction(),
                        new TurnRightInstruction(),
                        new TurnLeftInstruction(),
                        new TurnRightInstruction(),
                        new TurnLeftInstruction(),
                        new TurnRightInstruction(),
                    }),
            });

        public RobotsApplicationTests()
        {
            _marsSurfaceMock = new Mock<IMarsSurface>(MockBehavior.Default);
            _robotFactoryMock = new Mock<IRobotFactory>(MockBehavior.Default);
            _consoleWriterMock = new Mock<IConsoleWriter>(MockBehavior.Default);
            _inputParserMock = new Mock<IApplicationInputDataParser>(MockBehavior.Default);

            _SUT = new RobotsApplication(
                _marsSurfaceMock.Object,
                _robotFactoryMock.Object,
                _consoleWriterMock.Object,
                _inputParserMock.Object);
        }

        // Todo: write unit tests for RobotsApplication
        [Fact]
        public async Task RunAsync_Workflow_Correct()
        {
            // Arrange
            var args = new RobotsApplicationArguments() 
            { 
                InputFilePath = "./path.txt" 
            };

            // Act

            // Assert
        }
    }
}
