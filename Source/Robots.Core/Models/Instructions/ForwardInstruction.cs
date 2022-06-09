using Robots.Core.Enums;
using Robots.Core.Interfaces;

namespace Robots.Core.Models.Instructions
{
    public class ForwardInstruction : IRobotInstruction
    {
        public string Name => InstructionNames.Forward;

        public void ExecuteOn(IRobot robot)
        {
            if (robot.Orientation == Orientation.North)
            {
                robot.MoveBy(0, 1);
            }
            else if (robot.Orientation == Orientation.West)
            {
                robot.MoveBy(-1, 0);
            }
            else if (robot.Orientation == Orientation.South)
            {
                robot.MoveBy(0, -1);
            }
            else if (robot.Orientation == Orientation.East)
            {
                robot.MoveBy(1, 0);
            }
        }
    }
}
