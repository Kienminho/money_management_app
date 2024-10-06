using DomainService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModel;

namespace ABCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IHttpContextAccessor httpContextAccessor, IRoleService roleService) : base(
            httpContextAccessor)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Danh sách role
        /// </summary>
        /// <param name="keyword">Lọc theo activity name hoặc avtivity code</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="pageSize">Số lượng record trong 1 trang</param>
        /// <returns></returns>
        [HttpGet("get-list")]
        public async Task<IActionResult> GetListRole(string keyword = "", int pageIndex = 1, int pageSize = 20)
        {
            var res = await _roleService.GetListRole(keyword, pageIndex, pageSize);
            return Ok(res, new { keyword, pageIndex, pageSize }, res.Count);
        }

        /// <summary>
        /// Lấy role theo id
        /// </summary>
        /// <param name="model">Id của role (roleId)</param>
        /// <returns></returns>
        [HttpGet("get-list-by-id")]
        public async Task<IActionResult> GetRoleById(RoleIdRequest model)
        {
            var res = await _roleService.GetRoleById(model.Id);
            return Ok(res, model);
        }

        /// <summary>
        /// Thêm role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddRole(RoleRequest model)
        {
            var res = await _roleService.AddRole(Guid.Parse(userId ?? ""), model);
            return Ok(res, model);
        }

        /// <summary>
        /// Cập nhật role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> UpdateRole(UpdateRoleRequest model)
        {
            var res = await _roleService.UpdateRole(Guid.Parse(userId ?? ""), model);
            return Ok(res, model);
        }

        /// <summary>
        /// Xóa role
        /// </summary>
        /// <param name="model">Id của role (roleId)</param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRole(RoleIdRequest model)
        {
            var res = await _roleService.DeleteRole(Guid.Parse(userId ?? ""), model.Id);
            return Ok(res, model);
        }
    }
}