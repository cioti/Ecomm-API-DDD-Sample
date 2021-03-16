using Newtonsoft.Json;

namespace Ecomm.Shared.Models
{
    public class ApiResponse<T> : ApiResponse
    {
        public T Result { get; set; }

        private ApiResponse(T result, int statusCode = 200, string message = "", string apiVersion = "") : base(statusCode, message, apiVersion)
        {
            if (result != null)
            {
                Result = result;
            }
            else
            {
                Result = default;
            }
        }
        public ApiResponse() : base() { }
        private ApiResponse(ApiError apiError, int statusCode, string message = "", string apiVersion = "") : base(apiError, statusCode, message, apiVersion) { }

        public static ApiResponse<T> WithSuccess(T result, int httpStatusCode = 200, string message = "", string apiVersion = "") => new ApiResponse<T>(result, httpStatusCode, message, apiVersion);
        public static new ApiResponse<T> WithError(ApiError error, int httpStatusCode, string message = "", string apiVersion = "") => new ApiResponse<T>(error, httpStatusCode, message, apiVersion);
    }
    public class ApiResponse
    {
        public ApiResponse() { }

        internal ApiResponse(int statusCode = 200, string message = "", string apiVersion = "")
        {
            Message = message;
            ApiVersion = apiVersion;
            StatusCode = statusCode;
            Error = null;
            IsError = null;
        }
        internal ApiResponse(ApiError apiError, int statusCode, string message = "", string apiVersion = "")
        {
            Message = message;
            ApiVersion = apiVersion;
            StatusCode = statusCode;
            Error = apiError;
            IsError = true;
        }

        public string ApiVersion { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int StatusCode { get; set; }

        public string Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsError { get; set; }
        public ApiError Error { get; set; }

        public bool IsSuccessful() => (IsError == null || (IsError.HasValue && IsError.Value)) && Error == null;

        public static ApiResponse WithSuccess(int httpStatusCode = 200, string message = "", string apiVersion = "") => new ApiResponse(httpStatusCode, message, apiVersion);
        public static ApiResponse WithError(ApiError error, int httpStatusCode, string message = "", string apiVersion = "") => new ApiResponse(error, httpStatusCode, message, apiVersion);
    }
}
