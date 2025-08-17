using System.Net;

namespace MyDiary.API.Models
{
    public class APIResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public T Result { get; set; }
    }
}
