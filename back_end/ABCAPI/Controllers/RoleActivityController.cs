using DomainService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModel;

namespace ABCAPI.Controllers;

public class RoleActivityController : BaseController
{
    private readonly IRoleActivityService _roleActivityService;

    public RoleActivityController(IHttpContextAccessor httpContextAccessor, IRoleActivityService roleActivityService) : base(
        httpContextAccessor)
    {
        _roleActivityService = roleActivityService;
    }

    /// <summary>
    /// Lấy danh sách theo role id
    /// </summary>
    /// <param name="model">id của role (roleId)</param>
    /// <returns></returns>
    [HttpPost("get-list-by-roleId")]
    public async Task<IActionResult> GetListActivityOfRole(RoleIdRequest model)
    {
        var res = await _roleActivityService.GetListActivityOfRole(model.Id);
        return Ok(res, model, res.Count);
    }

    /// <summary>
    /// Cập nhật activity của role
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("update-activity-of-role")]
    public async Task<IActionResult> UpdateActivityOfRole(UpdateRoleActivityRequest model)
    {
        var res = await _roleActivityService.UpdateActivityOfRole(Guid.Parse(userId ?? ""), model);
        return Ok(res, model);
    }
}