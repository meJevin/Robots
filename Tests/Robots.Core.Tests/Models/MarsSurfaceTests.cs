using System.Linq;

namespace Robots.Core.Tests.Models
{
    public class MarsSurfaceTests
    {
        private readonly MarsSurface _SUT;

        public MarsSurfaceTests()
        {
            _SUT = new MarsSurface(new MarsSurfaceInitializeDataValidator());
        }

        [Fact]
        public void AddRobot_Throws_When_Not_Initialized()
        {
            // Arrange
            var robot = new Mock<IRobot>().Object;

            // Act
            var act = () => _SUT.AddRobot(robot);

            // Assert
            act.Should()
                .Throw<MarsSurfaceException>()
                .WithMessage("*hasn't been initialized*");
        }

        [Fact]
        public void AddRobot_Throws_When_Adding_Robot_That_Already_Exists()
        {
            // Arrange
            var robot = new Mock<IRobot>().Object;
            _SUT.Initialize(new(5, 5));
            _SUT.AddRobot(robot);

            // Act
            var act = () => _SUT.AddRobot(robot);

            // Assert
            act.Should()
                .Throw<MarsSurfaceException>()
                .WithMessage("*already exists*");
        }

        [Fact]
        public void AddRobot_Adds_Robot_When_Initialized_And_Robot_Does_Not_Already_Exist()
        {
            // Arrange
            var robot = new Mock<IRobot>().Object;
            _SUT.Initialize(new(5, 5));

            // Act
            _SUT.AddRobot(robot);

            // Assert
            _SUT.Robots.Should().HaveCount(1);
            _SUT.Robots.First().Should().Be(robot);
        }

        [Fact]
        public void AddProhibitedCell_Ignores_Cell_That_Already_Exist()
        {
            // Arrange
            var cell = new ProhibitedCell(3, 3, Orientation.South);
            _SUT.AddProhibitedCell(cell);

            // Act
            _SUT.AddProhibitedCell(cell);

            // Assert
            _SUT.ProhibitedCells.Should().HaveCount(1);
            _SUT.ProhibitedCells.First().Should().Be(cell);
        }

        [Fact]
        public void AddProhibitedCell_Adds_Cell_That_Does_Not_Already_Exist()
        {
            // Arrange
            var cell = new ProhibitedCell(3, 3, Orientation.South);

            // Act
            _SUT.AddProhibitedCell(cell);

            // Assert
            _SUT.ProhibitedCells.Should().HaveCount(1);
            _SUT.ProhibitedCells.First().Should().Be(cell);
        }

        [Fact]
        public void Initialize_Throws_If_Already_Initialized()
        {
            // Arrange
            _SUT.Initialize(new(1, 1));

            // Act
            var act = () => _SUT.Initialize(new(1,1));

            // Assert
            act.Should()
                .Throw<MarsSurfaceException>()
                .WithMessage("*already*initialized*");
        }

        [Fact]
        public void Initialize_Throws_If_Validation_Fails()
        {
            // Arrange
            var badData = new MarsSurfaceInitializeData(0, 0);

            // Act
            var act = () => _SUT.Initialize(badData);

            // Assert
            act.Should()
                .Throw<ValidationException>();
        }

        [Fact]
        public void Initialize_Initializes_Fields_Correctly()
        {
            // Arrange
            var data = new MarsSurfaceInitializeData(5, 5);

            // Act
            _SUT.Initialize(data);

            // Assert
            _SUT.Width.Should().Be(data.Width);
            _SUT.Height.Should().Be(data.Height);
        }
    }
}
