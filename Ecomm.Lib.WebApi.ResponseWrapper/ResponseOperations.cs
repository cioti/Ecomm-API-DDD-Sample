using Ecomm.Lib.WebApi.ResponseWrapper.Extensions;
using Ecomm.Shared.Exceptions;
using Ecomm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;


namespace Ecomm.Lib.WebApi.ResponseWrapper
{
    public class ResponseOperations
    {
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly WrapperOptions _options;
        private string ApiVersion => _options.ShowApiVersion ? _options.ApiVersion : null;
        public ResponseOperations(IOptions<WrapperOptions> options)
        {
            _options = options.Value;
            _jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = _options.UseCamelCaseNamingStrategy ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                NullValueHandling = _options.IgnoreNullValue ? NullValueHandling.Ignore : NullValueHandling.Include
            };
        }

        internal async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            var httpMethodsWithRequestBody = new[] { "POST", "PUT", "PATCH" };
            var hasRequestBody = httpMethodsWithRequestBody.Any(x => x.Equals(request.Method.ToUpper()));
            string requestBody = default;

            if (hasRequestBody)
            {
                request.EnableBuffering();

                using var memoryStream = new MemoryStream();
                await request.Body.CopyToAsync(memoryStream);
                requestBody = Encoding.UTF8.GetString(memoryStream.ToArray());
                request.Body.Seek(0, SeekOrigin.Begin);
            }
            return requestBody;
        }

        internal async Task<string> ReadResponseBodyStreamAsync(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);

            var (IsEncoded, ParsedText) = responseBody.VerifyBodyContent();

            return IsEncoded ? ParsedText : responseBody;
        }

        internal async Task RevertResponseBodyStreamAsync(Stream bodyStream, Stream orginalBodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            await bodyStream.CopyToAsync(orginalBodyStream);
        }

        internal bool IsResponseSuccessful(HttpContext context)
        {
            return context.Response.StatusCode >= 200 && context.Response.StatusCode < 400;
        }

        internal async Task HandleSuccessfulRequestAsync(HttpContext context, int httpStatusCode, object body)
        {
            var bodyText = !body.ToString().IsValidJson() ? ConvertToJSONString(body) : body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);

            var apiResponse = ApiResponse<object>.WithSuccess(bodyContent, httpStatusCode, $"{context.Request.Method} request successful", ApiVersion);
            var jsonString = ConvertToJSONString(apiResponse);

            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }

        internal async Task HandleUnsuccessfulRequestAsync(HttpContext context, int httpStatusCode, object body)
        {
            var bodyText = !body.ToString().IsValidJson() ? ConvertToJSONString(body) : body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);

            ApiError apiError = !string.IsNullOrEmpty(body.ToString()) ? new ApiError(bodyContent) : WrapUnsucessfulError(httpStatusCode);
            var jsonString = ConvertToJSONString(ApiResponse<object>.WithError(apiError, httpStatusCode, ResponseMessages.Failed, ApiVersion));

            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }

        internal async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ApiError apiError;
            int httpStatusCode = StatusCodes.Status500InternalServerError;
            if (ex is NotFoundException)
            {
                apiError = new ApiError(ex.Message);
                httpStatusCode = StatusCodes.Status404NotFound;
            }
            else if (ex is ValidationException)
            {
                apiError = new ApiError(ex.Message, (ex as ValidationException).Errors);
                httpStatusCode = StatusCodes.Status400BadRequest;
            }
            else if (ex is ForbiddenException)
            {
                apiError = new ApiError(ex.Message);
                httpStatusCode = StatusCodes.Status403Forbidden;
            }
            else
            {
                apiError = new ApiError("Unhandled exception occured. Unable to process request");
                if (_options.IsDebug)
                {
                    apiError.Details = ex.ToString();
                }
            }

            var jsonString = ConvertToJSONString(ApiResponse<object>.WithError(apiError, httpStatusCode, ResponseMessages.Failed, ApiVersion));

            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }

        private async Task WriteFormattedResponseToHttpContextAsync(HttpContext context, int httpStatusCode, string jsonString)
        {
            context.Response.StatusCode = httpStatusCode;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.ContentLength = jsonString != null ? Encoding.UTF8.GetByteCount(jsonString) : 0;
            await context.Response.WriteAsync(jsonString);
        }

        private ApiError WrapUnsucessfulError(int statusCode) =>
            statusCode switch
            {
                Status204NoContent => new ApiError(ResponseMessages.NotContent),
                Status400BadRequest => new ApiError(ResponseMessages.BadRequest),
                Status401Unauthorized => new ApiError(ResponseMessages.UnAuthorized),
                Status404NotFound => new ApiError(ResponseMessages.NotFound),
                Status405MethodNotAllowed => new ApiError(ResponseMessages.MethodNotAllowed),
                Status415UnsupportedMediaType => new ApiError(ResponseMessages.MediaTypeNotSupported),
                _ => new ApiError(ResponseMessages.Unknown)
            };

        private string ConvertToJSONString(object rawJSON) => JsonConvert.SerializeObject(rawJSON, _jsonSettings);
    }
}

