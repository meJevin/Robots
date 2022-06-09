using FluentValidation;
using Robots.Core.Enums;
using Robots.Core.Exceptions;

namespace Robots.Core.Models
{
    public record RobotInitializeData(int x, int y, Orientation orientation);

    public interface IRobot
    {
        int X { get; }
        int Y { get; }

        Orientation Orientation { get; }

        RobotState State { get; }

        /// <summary>
        /// Initializing robots is crutual for their operation, 
        /// otherwise they wouldn't know their initial position and orientation
        /// </summary>
        void Initialize(RobotInitializeData initializeData);

        void MoveBy(int x, int y);
        void ChangeOrientation(Orientation orientation);
    }

    public class Robot : IRobot
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Orientation Orientation { get; private set; }

        public RobotState State { get; private set; }

        private readonly IMarsSurface _marsSurface;
        private readonly IValidator<RobotInitializeData> _robotInitializationValidator;

        public Robot(IMarsSurface marsSurface, 
            IValidator<RobotInitializeData> robotInitializationValidator)
        {
            _marsSurface = marsSurface;
            _robotInitializationValidator = robotInitializationValidator;

            State = RobotState.Uninitialized;
        }

        public void Initialize(RobotInitializeData initializeData)
        {
            if (State != RobotState.Uninitialized)
            {
                throw new RobotException("Cannot initialize robot, which is already initialized");
            }

            _robotInitializationValidator.ValidateAndThrow(initializeData);

            if (initializeData.x > _marsSurface.Width ||
                initializeData.y > _marsSurface.Height)
            {
                throw new ArgumentException("Cannot initialize robot outside of mars surface");
            }

            X = initializeData.x;
            Y = initializeData.y;
            Orientation = initializeData.orientation;

            State = RobotState.Operational;
        }

        public void MoveBy(int x, int y)
        {
            if (State != RobotState.Operational) return;

            var isProhibitedToMove = _marsSurface.ProhibitedCells
                .FirstOrDefault(a => a.X == X && a.Y == Y && a.Orientation == Orientation) 
                is not null;

            if (isProhibitedToMove)
            {
                return;
            }

            if (X + x > _marsSurface.Width ||
                Y + y > _marsSurface.Height)
            {
                _marsSurface.AddProhibitedCell(new(X, Y, Orientation));
                State = RobotState.Lost;
                return;
            }

            X += x;
            Y += y;
        }

        public void ChangeOrientation(Orientation orientation)
        {
            if (State != RobotState.Operational) return;

            Orientation = orientation;
        }
    }
}
