
using FluentValidation;
using MediatR;

namespace Application.Behavior
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            // list of validators;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // if no validators next - break
            if (!_validators.Any())
            {
                return await next();
            }

            // wraping TRequest into ValidationContext
            var context = new ValidationContext<TRequest>(request);

            // selecting each ruleFor field for ValidationContext thar corresponds to his Validator
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );  
            // select all the errors
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();
            // if failures appear throw new exception where all the failures are stored
            if (failures.Count != 0)
            {
                throw new Application.Exceptions.ValidationException(failures);
            }

            return await next();
        }
    }
}
