using Robots.Core.Enums;
using Robots.Core.Interfaces;

namespace Robots.Application.Models
{
    public class ApplicationInputData
    {
        public MarsSurfaceInputData SurfaceData;
        public IList<RobotInputData> RobotInputDatas;

        public ApplicationInputData(
            MarsSurfaceInputData surfaceData, 
            IList<RobotInputData> robotinputDatas)
        {
            SurfaceData = surfaceData;
            RobotInputDatas = robotinputDatas;
        }
    }

    // Parsed from a single line in the following format:
    // 5 3 
    public class MarsSurfaceInputData
    {
        public int Width;
        public int Height;

        public MarsSurfaceInputData(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    // Parsed from lines in the following format:
    // 1 1 E
    // RFRFRFRF
    public class RobotInputData
    {
        public int X;
        public int Y;
        public Orientation Orientation;

        public IList<IRobotInstruction> Instructions;

        public RobotInputData(
            int x, int y, Orientation orientation, 
            IList<IRobotInstruction> instructions)
        {
            X = x;
            Y = y;
            Orientation = orientation;
            Instructions = instructions;
        }
    }
}
