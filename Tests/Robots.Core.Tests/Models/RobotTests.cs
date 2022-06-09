using System;
using System.Collections.Generic;

namespace Robots.Core.Tests.Models
{
    public class RobotTests
    {
        private readonly Mock<IMarsSurface> _marsSurfaceMock;
        private readonly Robot _SUT;

        public RobotTests()
        {
            _marsSurfaceMock = new Mock<IMarsSurface>(MockBehavior.Default);
            _SUT = new Robot(_marsSurfaceMock.Object, new RobotInitializeDataValidator());
        }

        [Fact]
        public void Initialize_Throws_If_Already_Initialized()
        {
            // Arrange
            _marsSurfaceMock.Setup(a => a.Width).Returns(5);
            _marsSurfaceMock.Setup(a => a.Height).Returns(5);

            _SUT.Initialize(new(0, 0, Orientation.South));

            // Act
            var act = () => _SUT.Initialize(new(0, 0, Orientation.South));

            // Assert
            act.Should()
                .Throw<RobotException>()
                .WithMessage("*already initialized*");
        }

        [Fact]
        public void Initialize_Throws_If_Validation_Fails()
        {
            // Arrange
            var data = new RobotInitializeData(-1, -4, Orientation.South);

            // Act
            var act = () => _SUT.Initialize(data);

            // Assert
            act.Should()
                .Throw<ValidationException>();
        }

        [Fact]
        public void Initialize_Throws_If_Outside_Of_Mars_Surface()
        {
            // Arrange
            _marsSurfaceMock.Setup(a => a.Width).Returns(5);
            _marsSurfaceMock.Setup(a => a.Height).Returns(5);

            // Act
            var act = () => _SUT.Initialize(new(6, 6, Orientation.South));

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("*outside of mars*");
        }

        [Fact]
        public void Initiaze_Initializes_Fields_Correctly()
        {
            // Arrange
            _marsSurfaceMock.Setup(a => a.Width).Returns(5);
            _marsSurfaceMock.Setup(a => a.Height).Returns(5);

            // Act
            _SUT.Initialize(new(5, 5, Orientation.South));

            // Assert
            _SUT.X.Should().Be(5);
            _SUT.Y.Should().Be(5);
            _SUT.Orientation.Should().Be(Orientation.South);
            _SUT.State.Should().Be(RobotState.Operational);
        }

        [Fact]
        public void MoveBy_Ignored_If_State_Not_Operational()
        {
            // Arrange
            var x = 1;
            var y = 2;

            // Act
            _SUT.MoveBy(x, y);
            
            // Assert
            _SUT.X.Should().Be(0);
            _SUT.Y.Should().Be(0);
        }

        [Fact]
        public void MoveBy_Ignored_If_Prohibited()
        {
            // Arrange
            _marsSurfaceMock.Setup(a => a.Width).Returns(5);
            _marsSurfaceMock.Setup(a => a.Height).Returns(5);

            var prohibitedCell = new ProhibitedCell(1, 2, Orientation.South);

            _marsSurfaceMock
                .Setup(a => a.ProhibitedCells)
                .Returns(new List<ProhibitedCell> { prohibitedCell });

            _SUT.Initialize(new RobotInitializeData(1, 2, Orientation.South));

            // Act
            _SUT.MoveBy(1, 0);

            // Assert
            _SUT.X.Should().Be(1);
            _SUT.Y.Should().Be(2);
        }

        [Fact]
        public void MoveBy_When_Outside_Of_Bounds_Marked_Lost_And_Marks_Cell_As_Prohibited()
        {
            // Arrange
            _marsSurfaceMock.Setup(a => a.Width).Returns(5);
            _marsSurfaceMock.Setup(a => a.Height).Returns(5);

            _marsSurfaceMock
                .Setup(a => a.ProhibitedCells)
                .Returns(new List<ProhibitedCell> { });

            _SUT.Initialize(new RobotInitializeData(5, 5, Orientation.South));

            // Act
            _SUT.MoveBy(1, 0);

            // Assert
            _marsSurfaceMock
                .Verify(a => a.AddProhibitedCell(new(5, 5, Orientation.South)), Times.Once);
            _SUT.X.Should().Be(5);
            _SUT.Y.Should().Be(5);
            _SUT.State.Should().Be(RobotState.Lost);
        }

        [Fact]
        public void MoveBy_Changes_Position_When_Move_Is_Within_Bounds()
        {
            // Arrange
            _marsSurfaceMock.Setup(a => a.Width).Returns(5);
            _marsSurfaceMock.Setup(a => a.Height).Returns(5);

            _marsSurfaceMock
                .Setup(a => a.ProhibitedCells)
                .Returns(new List<ProhibitedCell> { });

            _SUT.Initialize(new RobotInitializeData(5, 4, Orientation.South));

            // Act
            _SUT.MoveBy(0, 1);

            // Assert
            _SUT.X.Should().Be(5);
            _SUT.Y.Should().Be(5);
            _SUT.State.Should().Be(RobotState.Operational);
        }

        [Fact]
        public void ChangeOrientation_Ignored_If_State_Not_Operational()
        {
            // Arrange
            var newOrientation = Orientation.North;

            // Act
            _SUT.ChangeOrientation(newOrientation);
            
            // Assert
            _SUT.Orientation.Should().Be(Orientation.Unknown);
        }

        [Fact]
        public void ChangeOrientation_Changes_Orientation_If_State_Is_Operational()
        {
            // Arrange
            _marsSurfaceMock.Setup(a => a.Width).Returns(5);
            _marsSurfaceMock.Setup(a => a.Height).Returns(5);

            _SUT.Initialize(new RobotInitializeData(5, 5, Orientation.South));

            var newOrientation = Orientation.North;

            // Act
            _SUT.ChangeOrientation(newOrientation);

            // Assert
            _SUT.Orientation.Should().Be(newOrientation);
        }
    }
}
