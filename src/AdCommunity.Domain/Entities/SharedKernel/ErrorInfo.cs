namespace AdCommunity.Domain.Entities.SharedKernel;

public class ErrorInfo
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return $"{{ \"StatusCode\": {StatusCode}, \"Message\": \"{Message}\" }}";
    }
}
