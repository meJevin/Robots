using System;

namespace Robots.Application.Tests.Services
{
    public class RobotInstructionFactoryTests
    {
        private readonly RobotInstructionFactory _SUT;

        public RobotInstructionFactoryTests()
        {
            _SUT = new RobotInstructionFactory();
        }

        [Theory]
        [InlineData("F", typeof(ForwardInstruction))]
        [InlineData("L", typeof(TurnLeftInstruction))]
        [InlineData("R", typeof(TurnRightInstruction))]
        public void FromName_Produces_Correct_Instruction(
            string name, Type expectedInstructionType)
        {
            // Act
            var instruction = _SUT.FromName(name);

            // Assert
            instruction.Name.Should().Be(name);
            instruction.GetType().Should().Be(expectedInstructionType);
        }

        [Theory]
        [InlineData("H")]
        [InlineData("Y")]
        [InlineData("GFD")]
        [InlineData("SDF")]
        [InlineData("dfogjk")]
        [InlineData("dfog546")]
        [InlineData("dfog546 435sdf")]
        public void FromName_Throws_When_Provided_With_Unknown_Instruction_Name(string name)
        {
            // Act
            var act = () => _SUT.FromName(name);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("*Could not find robot instruction*");
        }
    }
}
