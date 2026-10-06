namespace Portfolio.Application.Common;

public sealed class NotFoundException(string resource, string key)
    : Exception($"{resource} '{key}' was not found.");

public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errors)
    : Exception("One or more validation errors occurred.")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
