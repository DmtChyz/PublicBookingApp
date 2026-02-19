using FluentValidation.Results;

namespace Application.Exceptions
{
    public class Exceptions : Exception
    {
        public Exceptions(string message) : base(message) { }
    }
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        // list of errors key/value pair
        public IDictionary<string, string[]> Errors { get; }

        // base : calling System.Exception class and passing name parameter
        public ValidationException() : base("Some validation failures occured")
        {
            Errors = new Dictionary<string, string[]>();
        }

        // Creating key/value pair dictionary with errors
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
