using Ecomm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.Lib.WebApi.Http
{
    /// <summary>
    /// Base HttpClient helper class to be used for inner service communication
    /// </summary>
    public class EcommHttpClient
    {
        private const string LogId = nameof(EcommHttpClient);
        private readonly ILogger<EcommHttpClient> _logger;
        private readonly HttpClient _client;

        public EcommHttpClient(HttpClient client, ILogger<EcommHttpClient> logger)
        {
            _client = client;
            _logger = logger;
            SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore
            };
        }
        public JsonSerializerSettings SerializerSettings { get; set; }

        private async Task<ApiResponse<T>> SendRequest<T>(
            HttpMethod verb,
            string requestUri,
            object requestBody = null)
        {
            HttpResponseMessage response;
            string responseBody;
            try
            {
                var message = new HttpRequestMessage(verb, new Uri(_client.BaseAddress, requestUri));
                var dataAsString = string.Empty;
                if (requestBody != null)
                {
                    dataAsString = JsonConvert.SerializeObject(requestBody, SerializerSettings);
                    message.Content = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                }
                _logger.LogTrace($"CdnHttpClient request {verb} {_client.BaseAddress + requestUri} Body:{dataAsString}");

                response = await _client.SendAsync(message).ConfigureAwait(false);
                responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogTrace($"CdnHttpClient response status code {response.StatusCode} | body {responseBody}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"CdnHttpClient failed to send request {_client.BaseAddress + requestUri}");
                return ApiResponse<T>.WithError(
                    new ApiError("Unknown problem occured while processing request. Please contact support"),
                    StatusCodes.Status500InternalServerError,
                    "Request failed");
            }
            if (response.IsSuccessStatusCode)
            {
                return HandleSuccessResponse<T>(responseBody, (int)response.StatusCode);
            }
            else
            {
                return HandleFailedResponse<T>(responseBody, (int)response.StatusCode);
            }
        }

        private ApiResponse<T> HandleFailedResponse<T>(string responseBody, int httpStatusCode)
        {
            if (!string.IsNullOrEmpty(responseBody))
            {
                try
                {
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody, SerializerSettings);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Attempt to deserialize {responseBody} into {typeof(T).Name} failed");
                    throw;
                }
            }
            return ApiResponse<T>.WithError(
                new ApiError("Unknown problem occured while processing request. Please contact support"),
                httpStatusCode,
                "Request failed");
        }

        private ApiResponse HandleFailedResponse(string responseBody, int httpStatusCode)
        {
            if (!string.IsNullOrEmpty(responseBody))
            {
                try
                {
                    return JsonConvert.DeserializeObject<ApiResponse>(responseBody, SerializerSettings);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Attempt to deserialize {responseBody} into {nameof(ApiResponse)} failed");
                    throw;
                }
            }
            return ApiResponse.WithError(
                new ApiError("Unknown problem occured while processing request. Please contact support"),
                httpStatusCode,
                "Request failed");

        }

        private ApiResponse<T> HandleSuccessResponse<T>(string responseBody, int httpStatusCode)
        {
            if (!string.IsNullOrEmpty(responseBody))
            {
                try
                {
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody, SerializerSettings);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Attempt to deserialize {responseBody} into {typeof(T).Name} failed");
                    throw;
                }
            }
            return ApiResponse<T>.WithSuccess(default, httpStatusCode, "Request successful");
        }

        private ApiResponse HandleSuccessResponse(string responseBody, int httpStatusCode)
        {
            if (!string.IsNullOrEmpty(responseBody))
            {
                try
                {
                    return JsonConvert.DeserializeObject<ApiResponse>(responseBody, SerializerSettings);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Attempt to deserialize {responseBody} into {nameof(ApiResponse)} failed");
                    throw;
                }
            }
            return ApiResponse.WithSuccess(httpStatusCode, "Request successful");
        }

        private async Task<ApiResponse> SendRequest(
            HttpMethod verb,
            string requestUri,
            object requestBody = null)
        {
            HttpResponseMessage response;
            string responseBody;
            try
            {
                var message = new HttpRequestMessage(verb, new Uri(_client.BaseAddress, requestUri));

                var dataAsString = string.Empty;
                if (requestBody != null)
                {
                    dataAsString = JsonConvert.SerializeObject(requestBody, SerializerSettings);
                    message.Content = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                }
                _logger.LogTrace($"CdnHttpClient request {verb} {_client.BaseAddress + requestUri} Body:{dataAsString}");

                response = await _client.SendAsync(message).ConfigureAwait(false);
                responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogTrace($"CdnHttpClient response {response.StatusCode} {responseBody}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"CdnHttpClient failed to send request {_client.BaseAddress + requestUri}");
                return ApiResponse.WithError(
                    new ApiError("Unknown problem occured while processing request. Please contact support"),
                    StatusCodes.Status500InternalServerError,
                    "Request failed");

            }

            if (response.IsSuccessStatusCode)
            {
                return HandleSuccessResponse(responseBody, (int)response.StatusCode);
            }
            else
            {
                return HandleFailedResponse(responseBody, (int)response.StatusCode);
            }
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string requestUri)
        {
            return await SendRequest<T>(HttpMethod.Get, requestUri).ConfigureAwait(false);
        }

        public async Task<ApiResponse> GetAsync(string requestUri)
        {
            return await SendRequest(HttpMethod.Get, requestUri).ConfigureAwait(false);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string requestUri, object requestBody)
        {
            return await SendRequest<T>(HttpMethod.Post, requestUri, requestBody).ConfigureAwait(false);
        }

        public async Task<ApiResponse> PostAsync(string requestUri, object requestBody)
        {
            return await SendRequest(HttpMethod.Post, requestUri, requestBody).ConfigureAwait(false);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string requestUri, object requestBody)
        {
            return await SendRequest<T>(HttpMethod.Put, requestUri, requestBody).ConfigureAwait(false);
        }

        public async Task<ApiResponse> PutAsync(string requestUri, object requestBody)
        {
            return await SendRequest(HttpMethod.Put, requestUri, requestBody).ConfigureAwait(false);
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string requestUri)
        {
            return await SendRequest<T>(HttpMethod.Delete, requestUri).ConfigureAwait(false);
        }

        public async Task<ApiResponse> DeleteAsync(string requestUri)
        {
            return await SendRequest(HttpMethod.Delete, requestUri).ConfigureAwait(false);
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string requestUri, object requestBody)
        {
            return await SendRequest<T>(HttpMethod.Delete, requestUri, requestBody).ConfigureAwait(false);
        }

        public async Task<ApiResponse> DeleteAsync(string requestUri, object requestBody)
        {
            return await SendRequest(HttpMethod.Delete, requestUri, requestBody).ConfigureAwait(false);
        }
    }

}
