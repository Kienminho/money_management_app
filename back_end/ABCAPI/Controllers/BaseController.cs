using System.Security.Claims;
using Common.Utils;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ABCAPI.Controllers;

public class BaseController : ControllerBase
{
    protected readonly ILog Log = LogManager.GetLogger(typeof(BaseController));
    protected string? userId;

    public BaseController(IHttpContextAccessor httpContextAccessor)
    {
        //var Id = User.Claims.Any() == null ? null : User.Claims?.Single(c => c.Type == "Id");
        userId = httpContextAccessor.HttpContext?.User == null
            ? ""
            : httpContextAccessor.HttpContext.User.FindFirstValue("Id");
    }

    protected OkObjectResult Ok([ActionResultObjectValue] object? value, int totalRecord = 0)
    {
        return Ok(value, new object(), totalRecord);
    }

    protected OkObjectResult Ok([ActionResultObjectValue] object? value, object body, int totalRecord = 0)
    {
        Utils.WriteLogInfo(Log, userId, Request, body);
        return base.Ok(Utils.CreateResponseModel(value, totalRecord));
    }

    protected OkObjectResult Error(Exception e)
    {
        Utils.WriteLogError(Log, userId, Request, e);
        return base.Ok(Utils.CreateErrorModel<object>(message: e.Message, exception: e));
    }

    /*protected OkObjectResult responseException(Exception e)
    {
        WriteLogError(e);
        return Ok(Utils.CreateErrorModel<object>(message: e.Message, exception: e));
    }

    protected OkObjectResult responseResult<T>(object body, BaseResponse<T> data) where T : class
    {
        WriteLogInfo(body);
        return Ok(data);
    }*/
}