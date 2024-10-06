using DomainService.Interfaces;
using Infrastructure.Implements;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModel;

namespace ABCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : BaseController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IHttpContextAccessor httpContextAccessor, IUserRoleService userRoleService) : base(
            httpContextAccessor)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetListUserRole(string keyword = "", int pageIndex = 1, int pageSize = 20)
        {
            var res = await _userRoleService.GetListUserRole(keyword, pageIndex, pageSize);
            return Ok(res, new { keyword, pageIndex, pageSize }, res.Count);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserRoleById(UserRoleIdRequest model)
        {
            var res = await _userRoleService.GetUserRoleById(model.Id);
            return Ok(res, model);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUserRole(UserRoleRequest model)
        {
            var res = await _userRoleService.AddUserRole(Guid.Parse(userId ?? ""), model);
            return Ok(res, model);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleRequest model)
        {
            var res = await _userRoleService.UpdateUserRole(Guid.Parse(userId ?? ""), model);
            return Ok(res, model);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUserRole(UserRoleIdRequest model)
        {
            var res = await _userRoleService.DeleteUserRole(Guid.Parse(userId ?? ""), model.Id);
            return Ok(res, model);
        }
    }
}
