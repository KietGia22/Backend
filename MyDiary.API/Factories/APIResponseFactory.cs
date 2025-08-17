using MyDiary.API.Models;
using System.Net;

namespace MyDiary.API.Factories
{
    public static class APIResponseFactory
    {
        public static APIResponse<T> Create<T>(HttpStatusCode statusCode, bool isSuccess, T result)
        {
            return new APIResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = isSuccess,
                Result = result
            };
        }
    }
}
