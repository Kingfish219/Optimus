namespace Optimus.Models
{
    public class Result
    {
        public bool Success { get; set; } = true;
        public string Title { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
    
    public class Result<T>
    {
        public bool Success { get; set; } = true;
        public string Title { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}
