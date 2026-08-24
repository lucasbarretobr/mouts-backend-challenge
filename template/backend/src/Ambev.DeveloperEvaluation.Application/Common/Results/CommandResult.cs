namespace Ambev.DeveloperEvaluation.Application.Common.Results;
public enum ErrorType 
{ 
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Failure 
} 

public sealed record CommandError(string Code,string Message,ErrorType Type=ErrorType.Failure); 
public sealed class CommandResult<T> { public bool IsSuccess {get;init;}
public T? Value {get;init;} public IReadOnlyCollection<CommandError> Errors {get;init;}=[];
public static implicit operator CommandResult<T>(T value)=>new(){IsSuccess=true,Value=value}; }
public sealed class CommandResult { public bool IsSuccess {get;init;}
public IReadOnlyCollection<CommandError> Errors {get;init;}=[]; }