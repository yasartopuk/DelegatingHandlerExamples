using System.Text.Json;

namespace DelegatingHandlerExamples.Shared.Models;

public class RequestResponseLogModel
{
    public string? RequestUri { get; set; }
    public string? Method { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public int StatusCode { get; set; }
    public TimeSpan TimeElapsed { get; set; }
    public string? TraceId { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
    }
}
