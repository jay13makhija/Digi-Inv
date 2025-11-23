namespace DigiInv.Application.Wrappers;

public class ApiResponse<T>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
    public T? Data { get; set; }

    public ApiResponse() { }

    public ApiResponse(T data, string message = null)
    {
        Succeeded = true;
        Message = message ?? "Success";
        Data = data;
    }

    public ApiResponse(string message)
    {
        Succeeded = false;
        Message = message;
    }
}

public class PagedResponse<T> : ApiResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        Message = "Success";
        Succeeded = true;
        Data = data;
    }
}
