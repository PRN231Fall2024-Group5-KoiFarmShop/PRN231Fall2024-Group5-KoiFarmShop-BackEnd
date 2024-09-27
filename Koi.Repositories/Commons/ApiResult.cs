namespace Koi.Repositories.Commons
{
    public record ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public static ApiResult<T> Succeed(T? data, string message)
        {
            return new ApiResult<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ApiResult<T> Error(T? data, string Message)
        {
            return new ApiResult<T> { IsSuccess = false, Data = data, Message = Message };
        }

        public static ApiResult<object> Fail(Exception ex)
        {
            return new ApiResult<object>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }
    }
}