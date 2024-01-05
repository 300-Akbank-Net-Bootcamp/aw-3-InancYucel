using System.Text.Json;

namespace Vb.Base.Response;

public class ApiResponse<T>
{
    public bool Sucsess { get; set; }
    public string Message { get; set; }
    public T Response { get; set; }
    
    public DateTime ServerData { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();

    public ApiResponse(bool isSucsess)
    {
        Sucsess = isSucsess;
        Response = default;
        Message = isSucsess ? "Success" : "Error";
    }

    public ApiResponse(T data)
    {
        Sucsess = true;
        Response = data;
        Message = "Success";
    }

    public ApiResponse(string message)
    {
        Sucsess = false;
        Response = default;
        Message = message;
    }
}

public class ApiResponse
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public ApiResponse(string message = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Success = true;
        }
        else
        {
            Success = false;
            Message = message;
        }
    }
    public bool Success { get; set; }
    public string Message { get; set; }
    public DateTime ServerData { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();
}