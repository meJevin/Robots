namespace Robots.Application.Exceptions
{
    public class ApplicationInputDataParserException : Exception
    {
        public ApplicationInputDataParserException() : base()
        {
        }

        public ApplicationInputDataParserException(string? message)
            : base(message)
        {
        }

        public ApplicationInputDataParserException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
