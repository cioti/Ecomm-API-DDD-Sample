namespace Ecomm.Lib.WebApi.ResponseWrapper
{
    public class WrapperOptions
    {
        /// <summary>
        /// Sets the Api version to be shown in the response. You must set the ShowApiVersion to true to see this value in the response.
        /// </summary>
        public string ApiVersion { get; set; } = "1.0.0.0";
        /// <summary>
        /// Shows the stack trace information in the responseException details.
        /// </summary>
        public bool IsDebug { get; set; } = false;
        /// <summary>
        /// Shows the Api Version attribute in the response.
        /// </summary>
        public bool ShowApiVersion { get; set; } = false;

        /// <summary>
        /// Tells the wrapper to ignore attributes with null values. Default is true.
        /// </summary>
        public bool IgnoreNullValue { get; set; } = true;

        /// <summary>
        /// Tells the wrapper to use camel case as the response format. Default is true.
        /// </summary>
        public bool UseCamelCaseNamingStrategy { get; set; } = true;

        /// <summary>
        /// Enables request logging and performance metrics
        /// </summary>
        public bool EnableRequestLogging { get; set; } = false;
    }
}
