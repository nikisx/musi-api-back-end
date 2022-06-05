namespace MusicApi.Common
{
    public class Response<T>
    {
        public string Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
