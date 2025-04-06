namespace Vocabu.Domain.DTOs;

public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ServiceResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Message = message, Data = data  };

    public static ServiceResponse<T> Error(string message) =>
        new() { Success = false, Message = message };
}
