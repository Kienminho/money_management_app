using DomainService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModel;

namespace ABCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivityController : BaseController
    {
        private readonly IUserActivityService _userActivityService;

        public UserActivityController(IUserActivityService userActivityService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _userActivityService = userActivityService;
        }

        /// <summary>
        /// Danh sách activity của user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("get-list-of-user")]
        public async Task<IActionResult> GetListActivityOfUser(UserIdRequest model)
        {
            var res = await _userActivityService.GetListActivityOfUser(model.Id);
            return Ok(res, model, res.Count);
        }

        /// <summary>
        /// Cập nhật activity của user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-activity-of-user")]
        public async Task<IActionResult> UpdateActivityOfUser(UpdateUserActivityRequest model)
        {
            var res = await _userActivityService.UpdateActivityOfUser(Guid.Parse(userId ?? ""), model);
            return Ok(res, model);
        }
    }
}
