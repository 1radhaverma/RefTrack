using FluentValidation;
using MediatR;

using RefTrack.Application.Common.Behaviours;

// RefTrack.Application / Common / Behaviours / ValidationBehaviour.cs
namespace RefTrack.Application.Common.Behaviours;

// Runs BEFORE every MediatR handler automatically
// SRP — validation is this class's only job
// DIP — depends on IValidator abstractions, not concrete validators
public class ValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(
        IEnumerable<IValidator<TRequest>>validators = null!)
    {
        _validators = validators ?? Enumerable.Empty<IValidator<TRequest>>();
    }
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v=>v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f=>f!=null)
            .ToList();


        // If any failures ? throw ? ExceptionMiddleware
        // catches ? returns 400 with error list
        if (failures.Count != 0)
            throw new ValidationException(failures);

        // No failures ? proceed to actual handler
        return await next();
    }

}