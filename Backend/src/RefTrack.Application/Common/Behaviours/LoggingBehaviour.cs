// RefTrack.Application/Common/Behaviours/LoggingBehaviour.cs
using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RefTrack.Application.Common.Behaviours;

// SRP — only logs, knows nothing about business logic
// Wraps EVERY handler automatically via MediatR pipeline
public class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        _logger.LogInformation(
            "Handling {Command}", name);

        var sw = Stopwatch.StartNew();
        var response = await next();  // run actual handler
        sw.Stop();

        _logger.LogInformation(
            "Handled {Command} in {Ms}ms",
            name, sw.ElapsedMilliseconds);

        return response;
    }
}