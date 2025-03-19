using System.Net;

namespace Vocabu.API.Common;

public class CommandResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    public static CommandResponse Ok(string? message = null) =>
        new() { Success = true, Message = message, StatusCode = (int)HttpStatusCode.OK };

    public static CommandResponse NotFound(string message) =>
        new() { Success = false, Message = message, StatusCode = (int)HttpStatusCode.NotFound };

    public static CommandResponse BadRequest(string message) =>
        new() { Success = false, Message = message, StatusCode = (int)HttpStatusCode.BadRequest };

    public static CommandResponse Conflict(string message) =>
        new() { Success = false, Message = message, StatusCode = (int)HttpStatusCode.Conflict };

    public static CommandResponse Unauthorized(string message) =>
        new() { Success = false, Message = message, StatusCode = (int)HttpStatusCode.Unauthorized };

    public static CommandResponse Forbidden(string message) =>
        new() { Success = false, Message = message, StatusCode = (int)HttpStatusCode.Forbidden };

    public static CommandResponse InternalServerError(string message) =>
        new() { Success = false, Message = message, StatusCode = (int)HttpStatusCode.InternalServerError };

    public static CommandResponse ValidatorError(IEnumerable<string> message) =>
        new() { Success = false, Message = string.Join(" \n ", message), (int)HttpStatusCode.Conflict };

    public static CommandResponse Error(string message, HttpStatusCode statusCode) =>
        new() { Success = false, Message = message, StatusCode = (int)statusCode };
}

public class CommandResponse<T> : CommandResponse
{
    public T? Data { get; set; }

    public static CommandResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Message = message, Data = data, StatusCode = (int)HttpStatusCode.OK };

    public static new CommandResponse<T> Error(string message, HttpStatusCode statusCode) =>
        new() { Success = false, Message = message, StatusCode = (int)statusCode };
}