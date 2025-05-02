namespace KmtBackend.API.Common
{
    public class ResponseWrapper(object data, string message, bool success, List<string>? errors = null)
    {
        public object Data { get; } = data;
        public string Message { get; } = message;
        public bool Success { get; } = success;
        public List<string>? Errors { get; } = errors;
    }
}
