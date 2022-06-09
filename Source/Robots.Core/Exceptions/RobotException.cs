namespace Robots.Core.Exceptions
{
    public class RobotException : Exception
    {
        public RobotException() : base()
        {
        }

        public RobotException(string? message)
            : base(message)
        {
        }

        public RobotException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
