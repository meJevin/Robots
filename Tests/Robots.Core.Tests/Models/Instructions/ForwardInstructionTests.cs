namespace Robots.Core.Tests.Models.Instructions
{
    public class ForwardInstructionTests
    {
        private readonly Mock<IRobot> _robotMock;
        private readonly ForwardInstruction _SUT;
        
        public ForwardInstructionTests()
        {
            _SUT = new ForwardInstruction();
            _robotMock = new Mock<IRobot>(MockBehavior.Default);
        }

        [Fact]
        public void Name_Is_Correct()
        {
            _SUT.Name.Should().Be(InstructionNames.Forward);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_North_Moves_Up()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.North);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.MoveBy(0, 1), Times.Once);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_West_Moves_Left()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.West);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.MoveBy(-1, 0), Times.Once);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_South_Moves_Down()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.South);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.MoveBy(0, -1), Times.Once);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_East_Moves_Right()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.East);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.MoveBy(1, 0), Times.Once);
        }

        [Fact]
        public void ExecuteOn_Robot_With_Orientation_Unknown_Does_Not_Move()
        {
            // Arrange
            _robotMock.Setup(a => a.Orientation).Returns(Orientation.Unknown);

            // Act
            _SUT.ExecuteOn(_robotMock.Object);

            // Assert
            _robotMock.Verify(a => a.MoveBy(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
