using System.Net;
using System.Text.Json;
using Common.Utils;
using log4net;
using Microsoft.AspNetCore.Http;

namespace Common.Authorization;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILog log = LogManager.GetLogger(typeof(ErrorHandlerMiddleware));

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Intercept the response
        var originalBodyStream = context.Response.Body;
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            if (context.Response.StatusCode != (int)HttpStatusCode.OK)
            {
                var response = context.Response;

                // Write log
                var httpStatusCode = (HttpStatusCode)response.StatusCode;
                var id = context.User == null ? null : context.User.FindFirst("Id");
                var userId = id != null ? id.Value : null;
                var apiRequested = context.Request.Path.Value ?? "";
                log.Error(
                    $"USERID: {userId} - URL: {apiRequested} failed. - BODY:  - MESSAGE: {httpStatusCode.ToString()}");

                // Response
                var dataRes = new BaseResponse<object>();
                dataRes.StatusCode = response.StatusCode;
                dataRes.Message = httpStatusCode.ToString();

                response.ContentType = "application/json";

                // Serialize the dataRes object to the response stream
                response.Body.SetLength(0);
                await response.WriteAsync(JsonSerializer.Serialize(dataRes));
            }
        }
        catch (Exception error)
        {
            var response = context.Response;

            // Write log
            var id = context.User == null ? null : context.User.FindFirst("Id");
            var userId = id != null ? id.Value : null;
            var apiRequested = context.Request.Path.Value ?? "";
            log.Error($"USERID: {userId} - URL: {apiRequested} failed. - BODY:  - MESSAGE: {error?.Message}");

            var dataRes = new BaseResponse<object>
            {
                Message = error?.Message
            };

            switch (error)
            {
                case AppException e:
                    // Custom application error
                    dataRes.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // Not found error
                    dataRes.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // Unhandled error
                    dataRes.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "application/json";
            await response.WriteAsync(JsonSerializer.Serialize(dataRes));
        }
        finally
        {
            // Copy the modified response to the original response body stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}