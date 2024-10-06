using DomainService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModel;

namespace ABCAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : BaseController
{
    private readonly IActivityService _activityService;

    public ActivityController(IHttpContextAccessor httpContextAccessor, IActivityService activityService) : base(
        httpContextAccessor)
    {
        _activityService = activityService;
    }

    /// <summary>
    /// Danh sách activity
    /// </summary>
    /// <param name="keyword">Lọc theo activity name hoặc avtivity code</param>
    /// <param name="pageIndex">Vị trí trang</param>
    /// <param name="pageSize">Số lượng record trong 1 trang</param>
    /// <returns></returns>
    [HttpGet("get-list")]
    public async Task<IActionResult> GetListActivity(string keyword = "", int pageIndex = 1, int pageSize = 20)
    {
        var res = await _activityService.GetListActivity(keyword, pageIndex, pageSize);
        return Ok(res, new {keyword, pageIndex, pageSize}, res.Count);
    }

    /// <summary>
    /// Lấy activity theo id
    /// </summary>
    /// <param name="model">Id của activity (ActivityId)</param>
    /// <returns></returns>
    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetActivityById(ActivityIdRequest model)
    {
        var res = await _activityService.GetActivityById(model.Id);
        return Ok(res, model);
    }

    /// <summary>
    /// Thêm activity
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("add")]
    public async Task<IActionResult> AddActivity(ActivityRequest model)
    {
        var res = await _activityService.AddActivity(Guid.Parse(userId ?? ""), model);
        return Ok(res, model);
    }

    /// <summary>
    /// Cập nhật activity
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("update")]
    public async Task<IActionResult> UpdateActivity(UpdateActivityRequest model)
    {
        var res = await _activityService.UpdateActivity(Guid.Parse(userId ?? ""), model);
        return Ok(res, model);
    }

    /// <summary>
    /// Xóa activity
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteActivity(ActivityIdRequest model)
    {
        var res = await _activityService.DeleteActivity(Guid.Parse(userId ?? ""), model.Id);
        return Ok(res, model);
    }
}