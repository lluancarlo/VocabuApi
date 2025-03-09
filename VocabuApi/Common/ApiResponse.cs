namespace VocabuApi.Common;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public static ApiResponse Ok(string? message = null) =>
        new() { Success = true, Message = message};

    public static ApiResponse Error(string message) =>
        new() { Success = false, Message = message };
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Error(string message) =>
        new() { Success = false, Message = message };
}
