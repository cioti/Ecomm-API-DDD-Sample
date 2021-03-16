using Ecomm.Lib.WebApi.ResponseWrapper.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Ecomm.Lib.WebApi.ResponseWrapper
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ResponseOperations _operations;
        private readonly WrapperOptions _options;
        private readonly ILogger<ResponseWrapperMiddleware> _logger;
        public ResponseWrapperMiddleware(RequestDelegate next,
            ResponseOperations responseOperations,
            IOptions<WrapperOptions> options,
            ILogger<ResponseWrapperMiddleware> logger)
        {
            _options = options.Value;
            _logger = logger;
            _next = next;
            _operations = responseOperations;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.IsApiRequest() || context.IsSwagger())
            {
                await _next(context);
            }
            else
            {
                var stopWatch = Stopwatch.StartNew();
                var originalResponseBodyStream = context.Response.Body;
                var requestBody = await _operations.GetRequestBodyAsync(context.Request);
                using var memoryStream = new MemoryStream();

                try
                {
                    context.Response.Body = memoryStream;
                    await _next(context);

                    var actionIgnore = context.Request.Headers["ResponseWrapperIgnore"];
                    if (actionIgnore.Count > 0)
                    {
                        await _operations.RevertResponseBodyStreamAsync(memoryStream, originalResponseBodyStream);
                        return;
                    }

                    var textBody = await _operations.ReadResponseBodyStreamAsync(memoryStream);
                    context.Response.Body = originalResponseBodyStream;

                    if (context.Response.StatusCode != Status304NotModified && context.Response.StatusCode != Status204NoContent)
                    {
                        if (_operations.IsResponseSuccessful(context))
                        {
                            await _operations.HandleSuccessfulRequestAsync(context, context.Response.StatusCode, textBody);
                        }
                        else
                        {
                            await _operations.HandleUnsuccessfulRequestAsync(context, context.Response.StatusCode, textBody);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{context.Request.Host}{context.Request.Path} {context.Request.Method} failed.");
                    await _operations.HandleExceptionAsync(context, ex);
                    await _operations.RevertResponseBodyStreamAsync(memoryStream, originalResponseBodyStream);
                }
                finally
                {
                    LogHttpRequest(context, requestBody, stopWatch);
                }
            }
        }
        private void LogHttpRequest(HttpContext context, string requestBody, Stopwatch stopWatch)
        {
            stopWatch.Stop();
            if (_options.EnableRequestLogging)
            {
                var request = $"{context.Request.Method} " +
                    $"{context.Request.Scheme} " +
                    $"{context.Request.Host}{context.Request.Path} " +
                    $"{context.Request.QueryString} " +
                    $"{requestBody}";

                _logger.Log(LogLevel.Information, $"Source:[{context.Connection.RemoteIpAddress }] " +
                                                  $"Request: {request} " +
                                                  $"Responded with [{context.Response.StatusCode}] in {stopWatch.ElapsedMilliseconds}ms");
            }
        }
    }
}
