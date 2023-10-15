namespace DelegatingHandlerExamples.Shared.Contexts;

public class TraceContext : IDisposable
{
    private static readonly AsyncLocal<TraceContext> _current = new AsyncLocal<TraceContext>();
    public string TraceId { get; set; }

    public static TraceContext Current
    {
        get { return _current.Value; }
        private set { _current.Value = value; }
    }

    private TraceContext() { }

    public static TraceContext Begin(string? traceId = null)
    {
        if (string.IsNullOrEmpty(traceId))
        {
            traceId = Guid.NewGuid().ToString().ToUpper();
        }

        Current = new TraceContext();
        Current.TraceId = traceId;
        return Current;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Current = null;
    }
}