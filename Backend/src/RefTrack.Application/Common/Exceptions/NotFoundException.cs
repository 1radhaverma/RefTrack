// RefTrack.Application/Common/Exceptions/NotFoundException.cs
namespace RefTrack.Application.Common.Exceptions;

// SRP — only represents "entity not found" scenario
// ExceptionMiddleware catches this ? returns HTTP 404
public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with id '{key}' was not found.")
    { }
}