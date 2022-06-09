using Robots.Application.Tests.Mock;
using System;
using System.Threading.Tasks;

namespace Robots.Application.Tests.Services
{
    public class ApplicationInputDataParserTests
    {
        private Mock<IRobotInstructionFactory> _instructionFactoryMock;
        private Mock<IFileReader> _fileReaderMock;
        private Mock<ApplicationInputDataParser> _SUT;

        public ApplicationInputDataParserTests()
        {
            _instructionFactoryMock = new Mock<IRobotInstructionFactory>(MockBehavior.Default);
            _fileReaderMock = new Mock<IFileReader>(MockBehavior.Default);

            _SUT = new Mock<ApplicationInputDataParser>(
                _instructionFactoryMock.Object, _fileReaderMock.Object);
        }

        [Fact]
        public async Task FromStringAsync_Parses_Application_Input_Data_Correctly()
        {
            // Arrange
            _SUT.Setup(a => a.ParseMarsSurfaceInputData(It.IsAny<string>())).CallBase();
            _SUT.Setup(a => a.ParseRobotInputData(It.IsAny<string>(), It.IsAny<string>())).CallBase();
            _SUT.Setup(a => a.FromStringAsync(It.IsAny<string>())).CallBase();

            var inputText = MockData.MockApplicationInputText;

            // Act
            var result = await _SUT.Object.FromStringAsync(inputText);

            // Assert
            result.SurfaceData.Width.Should().Be(5);
            result.SurfaceData.Height.Should().Be(3);

            result.RobotInputDatas.Count.Should().Be(3);

            result.RobotInputDatas[0].X.Should().Be(1);
            result.RobotInputDatas[0].Y.Should().Be(1);
            result.RobotInputDatas[0].Orientation.Should().Be(Orientation.East);

            result.RobotInputDatas[1].X.Should().Be(3);
            result.RobotInputDatas[1].Y.Should().Be(2);
            result.RobotInputDatas[1].Orientation.Should().Be(Orientation.North);

            result.RobotInputDatas[2].X.Should().Be(0);
            result.RobotInputDatas[2].Y.Should().Be(3);
            result.RobotInputDatas[2].Orientation.Should().Be(Orientation.West);
        }

        [Fact]
        public async Task FromFileAsync_Calls_FromStringAsync_With_Result_From_FileReader()
        {
            // Arrange
            var fileName = "someFile.txt";
            var textFromFile = "text";

            _fileReaderMock.Setup(a => a.ReadAllTextAsync(fileName)).ReturnsAsync(textFromFile);

            // Act
            await _SUT.Object.FromFileAsync(fileName);

            // Assert
            _fileReaderMock.Verify(a => a.ReadAllTextAsync(fileName), Times.Once);
            _SUT.Verify(a => a.FromStringAsync(textFromFile), Times.Once);
        }

        [Fact]
        public async Task FromStringAsync_Throws_When_No_Mars_Surface_Data_Provided()
        {
            // Arrange
            _SUT.Setup(a => a.FromStringAsync(It.IsAny<string>())).CallBase();

            var inputText = "";

            // Act
            var act = () => _SUT.Object.FromStringAsync(inputText);

            // Assert
            await act.Should()
                .ThrowAsync<ApplicationInputDataParserException>()
                .WithMessage("*surface data not provided*");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1 2 3")]
        [InlineData("1 2 3 4")]
        [InlineData("1 2 3 4 5")]
        [InlineData("1 sdfg gdfghs")]
        public void ParseMarsSurfaceInputData_Throws_When_Incorrect_Format_Provided(string inputText)
        {
            // Arrange
            _SUT.Setup(a => a.ParseMarsSurfaceInputData(It.IsAny<string>())).CallBase();

            // Act
            var act = () => _SUT.Object.ParseMarsSurfaceInputData(inputText);

            // Assert
            act.Should()
                .Throw<ApplicationInputDataParserException>()
                .WithMessage("*Incorrect format*");
        }

        [Theory]
        [InlineData("f 4")]
        [InlineData("fdfgfgh 4")]
        [InlineData("dfgdff 4")]
        public void ParseMarsSurfaceInputData_Throws_When_Width_Is_Not_An_Integer(string inputText)
        {
            // Arrange
            _SUT.Setup(a => a.ParseMarsSurfaceInputData(It.IsAny<string>())).CallBase();

            // Act
            var act = () => _SUT.Object.ParseMarsSurfaceInputData(inputText);

            // Assert
            act.Should()
                .Throw<ApplicationInputDataParserException>()
                .WithMessage("*Could not parse width value*");
        }

        [Theory]
        [InlineData("4 f")]
        [InlineData("3 fdfgfgh")]
        [InlineData("1 dfgdff")]
        public void ParseMarsSurfaceInputData_Throws_When_Height_Is_Not_An_Integer(string inputText)
        {
            // Arrange
            _SUT.Setup(a => a.ParseMarsSurfaceInputData(It.IsAny<string>())).CallBase();

            // Act
            var act = () => _SUT.Object.ParseMarsSurfaceInputData(inputText);

            // Assert
            act.Should()
                .Throw<ApplicationInputDataParserException>()
                .WithMessage("*Could not parse height value*");
        }

        [Theory]
        [InlineData("4 3", 4, 3)]
        [InlineData("3 5", 3, 5)]
        [InlineData("11 7", 11, 7)]
        public void ParseMarsSurfaceInputData_Parses_Mars_Surface_Data_Correctly(
            string inputText, int expectedWidth, int expectedHeight)
        {
            // Arrange
            _SUT.Setup(a => a.ParseMarsSurfaceInputData(It.IsAny<string>())).CallBase();

            // Act
            var result = _SUT.Object.ParseMarsSurfaceInputData(inputText);

            // Assert
            result.Width.Should().Be(expectedWidth);
            result.Height.Should().Be(expectedHeight);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1 2")]
        [InlineData("1 2 3 4")]
        [InlineData("1 2 3 4 5")]
        public void ParseRobotInputData_Throws_When_Incorrect_Format_Provided(string inputPos)
        {
            // Arrange
            _SUT.Setup(a => a.ParseRobotInputData(It.IsAny<string>(), It.IsAny<string>()))
                .CallBase();

            // Act
            var act = () => _SUT.Object.ParseRobotInputData(inputPos, "");

            // Assert
            act.Should()
                .Throw<ApplicationInputDataParserException>()
                .WithMessage("*Incorrect format*");
        }

        [Theory]
        [InlineData("dfg 1 E")]
        [InlineData("fgfgh 1 E")]
        [InlineData("ad3fvb 1 E")]
        public void ParseRobotInputData_Throws_When_X_Is_Not_An_Integer(string inputPos)
        {
            // Arrange
            _SUT.Setup(a => a.ParseRobotInputData(It.IsAny<string>(), It.IsAny<string>()))
                .CallBase();

            // Act
            var act = () => _SUT.Object.ParseRobotInputData(inputPos, "");

            // Assert
            act.Should()
                .Throw<ApplicationInputDataParserException>()
                .WithMessage("*Could not parse position X value*");
        }

        [Theory]
        [InlineData("1 dfg E")]
        [InlineData("1 fgfgh E")]
        [InlineData("1 ad3fvb E")]
        public void ParseRobotInputData_Throws_When_Y_Is_Not_An_Integer(string inputPos)
        {
            // Arrange
            _SUT.Setup(a => a.ParseRobotInputData(It.IsAny<string>(), It.IsAny<string>()))
                .CallBase();

            // Act
            var act = () => _SUT.Object.ParseRobotInputData(inputPos, "");

            // Assert
            act.Should()
                .Throw<ApplicationInputDataParserException>()
                .WithMessage("*Could not parse position Y value*");
        }

        [Theory]
        [InlineData("1 2 dfg")]
        [InlineData("1 2 fgfgh")]
        [InlineData("1 2 ad3fvb")]
        public void ParseRobotInputData_Throws_When_Orientation_Is_Invalid(string inputPos)
        {
            // Arrange
            _SUT.Setup(a => a.ParseRobotInputData(It.IsAny<string>(), It.IsAny<string>()))
                .CallBase();

            // Act
            var act = () => _SUT.Object.ParseRobotInputData(inputPos, "");

            // Assert
            act.Should()
                .Throw<ApplicationInputDataParserException>()
                .WithMessage("*Could not parse orientation value*");
        }

        [Theory]
        [InlineData("1 2 E", "KOSDGJI")]
        [InlineData("1 2 E", "VKFDIOSU")]
        [InlineData("1 2 E", "EROFSDFG")]
        public void ParseRobotInputData_Throws_When_Instruction_Name_Is_Invalid(
            string inputPos, string instructionsLine)
        {
            // Arrange
            _SUT = new Mock<ApplicationInputDataParser>(new RobotInstructionFactory());
            _SUT.Setup(a => a.ParseRobotInputData(It.IsAny<string>(), It.IsAny<string>()))
                .CallBase();

            // Act
            var act = () => _SUT.Object.ParseRobotInputData(inputPos, instructionsLine);

            // Assert
            act.Should()
                .Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("1 2 E", "FFFFFFFFFFFFF", 1, 2, Orientation.East)]
        [InlineData("1 2 E", "FRFRLFRLFRLFRLF", 1, 2, Orientation.East)]
        [InlineData("1 2 E", "LLLLLLRRRRFFFFFFFF", 1, 2, Orientation.East)]
        public void ParseRobotInputData_Parses_Robot_Input_Data_Correctly(
            string inputPos, string instructionsLine,
            int expectedX, int expectedY, Orientation expectedOrientation)
        {
            // Arrange
            _SUT = new Mock<ApplicationInputDataParser>(new RobotInstructionFactory(), new FileReader());
            _SUT.Setup(a => a.ParseRobotInputData(It.IsAny<string>(), It.IsAny<string>()))
                .CallBase();

            // Act
            var result = _SUT.Object.ParseRobotInputData(inputPos, instructionsLine);

            // Assert
            result.X.Should().Be(expectedX);
            result.Y.Should().Be(expectedY);
            result.Orientation.Should().Be(expectedOrientation);
            result.Instructions.Count.Should().Be(instructionsLine.Length);
            for (int i = 0; i < instructionsLine.Length; i++)
            {
                result.Instructions[i].Name.Should().Be(instructionsLine[i].ToString());
            }
        }
    }
}
