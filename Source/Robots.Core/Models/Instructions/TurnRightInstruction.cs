using Robots.Core.Enums;
using Robots.Core.Interfaces;

namespace Robots.Core.Models.Instructions
{
    public class TurnRightInstruction : IRobotInstruction
    {
        public string Name => InstructionNames.TurnRight;

        public void ExecuteOn(IRobot robot)
        {
            if (robot.Orientation == Orientation.North)
            {
                robot.ChangeOrientation(Orientation.East);
            }
            else if (robot.Orientation == Orientation.East)
            {
                robot.ChangeOrientation(Orientation.South);
            }
            else if (robot.Orientation == Orientation.South)
            {
                robot.ChangeOrientation(Orientation.West);
            }
            else if (robot.Orientation == Orientation.West)
            {
                robot.ChangeOrientation(Orientation.North);
            }
        }
    }
}
