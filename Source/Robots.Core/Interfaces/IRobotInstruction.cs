using Robots.Core.Models;

namespace Robots.Core.Interfaces
{
    public interface IRobotInstruction
    {
        string Name { get; }

        void ExecuteOn(IRobot robot);
    }
}
