
namespace Ecomm.Lib.WebApi.Http
{
    public class HttpClientSettings
    {
        public string Url { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelayInMs { get; set; }

        public HttpClientSettings()
        {
            Url = "http:\\\\localhost:5000";
            RetryCount = 0;
            RetryDelayInMs = 500;
        }
    }
}
