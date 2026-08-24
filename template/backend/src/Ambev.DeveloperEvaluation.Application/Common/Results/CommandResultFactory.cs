namespace Ambev.DeveloperEvaluation.Application.Common.Results;

public static class CommandResultFactory
{
    public static CommandResult<T> Success<T>(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static CommandResult<T> Failure<T>(params CommandError[] errors) => new()
    {
        IsSuccess = false,
        Errors = errors
    };

    public static CommandResult Success() => new()
    {
        IsSuccess = true
    };

    public static CommandResult Failure(params CommandError[] errors) => new()
    {
        IsSuccess = false,
        Errors = errors
    };

    public static CommandResult<T> NotFound<T>(string code, string message) =>
        Failure<T>(new CommandError(code, message, ErrorType.NotFound));

    public static CommandResult<T> Unauthorized<T>(string code, string message) =>
        Failure<T>(new CommandError(code, message, ErrorType.Unauthorized));

    public static CommandError Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static CommandError NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static CommandError Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static CommandError Unauthorized(string code, string message) =>
        new(code, message, ErrorType.Unauthorized);

    public static CommandError Unexpected(string code, string message) =>
        new(code, message, ErrorType.Failure);
}
