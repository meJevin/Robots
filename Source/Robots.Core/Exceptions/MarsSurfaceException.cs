namespace Robots.Core.Exceptions
{
    public class MarsSurfaceException : Exception
    {
        public MarsSurfaceException() : base()
        {
        }

        public MarsSurfaceException(string? message)
            : base(message)
        {
        }

        public MarsSurfaceException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
