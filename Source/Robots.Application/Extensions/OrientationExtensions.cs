using Robots.Core;
using Robots.Core.Enums;

namespace Robots.Application.Extensions
{
    public static class OrientationExtensions
    {
        public static string ToReadable(this Orientation orientation)
        {
            if (orientation == Orientation.North)
            {
                return OrientationNames.North;
            }
            else if (orientation == Orientation.West)
            {
                return OrientationNames.West;
            }
            else if (orientation == Orientation.South)
            {
                return OrientationNames.South;
            }
            else if (orientation == Orientation.East)
            {
                return OrientationNames.East;
            }

            return OrientationNames.Unknown;
        }

        public static bool TryParse(string orientationStr, out Orientation orientation)
        {
            if (orientationStr == OrientationNames.North)
            {
                orientation = Orientation.North;
                return true;
            }
            else if (orientationStr == OrientationNames.West)
            {
                orientation = Orientation.West;
                return true;
            }
            else if (orientationStr == OrientationNames.South)
            {
                orientation = Orientation.South;
                return true;
            }
            else if (orientationStr == OrientationNames.East)
            {
                orientation = Orientation.East;
                return true;
            }

            orientation = Orientation.Unknown;
            return false;
        }
    }
}
