namespace Robots.Core.Tests.Models.Instructions
{
    public class TurnRightInstructionTests
    {
        private readonly Mock<IRobot> _robotMock;
        private readonly TurnRightInstruction _SUT;

        public TurnRightInstructionTests()
        {
            _SUT = new TurnRightInstruction();
            _robotMock = new Mock<IRobot>(MockBehavior.Default);
        }

        [Fact]
        public void Name_Is_Correct()
        {
            _SUT.Name.Should().Be(InstructionNames.TurnRight);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_North_Changes_Orientation_To_East()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.North);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.ChangeOrientation(Orientation.East), Times.Once);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_East_Changes_Orientation_To_South()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.East);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.ChangeOrientation(Orientation.South), Times.Once);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_South_Changes_Orientation_To_West()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.South);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.ChangeOrientation(Orientation.West), Times.Once);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_West_Changes_Orientation_To_North()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.West);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.ChangeOrientation(Orientation.North), Times.Once);
        }
    }
}
