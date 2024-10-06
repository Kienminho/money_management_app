using Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Common.Authorization
{
    public class Authorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public string? role { get; set; }
        public string? activityCode { get; set; }
        public ActivityType activity { get; set; }
        private readonly ClaimsPrincipal _caller;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            /*try
            {
                // skip authorization if action is decorated with [AllowAnonymous] attribute
                var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
                if (allowAnonymous)
                    return;

                if (!string.IsNullOrWhiteSpace(role))
                {
                    var roleName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                    var deserializeRole = roleName.Value;
                    if (deserializeRole == role)
                        return;
                }

                if (!string.IsNullOrWhiteSpace(activityCode))
                {
                    var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");
                    var deserializeId = userId?.Value;
                    var res = false;
                    using (var _context = new MyContext(connectionString: ConfigurationManager.ConnectionStrings["WebApiDatabase"].ConnectionString))
                    {
                        var objCreate = new SqlParameter()
                        {
                            ParameterName = "@Create",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        var objRead = new SqlParameter()
                        {
                            ParameterName = "@Read",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        var objUpdate = new SqlParameter()
                        {
                            ParameterName = "@Update",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        var objDelete = new SqlParameter()
                        {
                            ParameterName = "@Delete",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        var userIdParameter = !string.IsNullOrEmpty(deserializeId)
                            ? new SqlParameter()
                            {
                                ParameterName = "@UserId",
                                SqlDbType = SqlDbType.UniqueIdentifier,
                                Value = Guid.Parse(deserializeId)
                            }
                            : new SqlParameter()
                            {
                                ParameterName = "@UserId",
                                SqlDbType = SqlDbType.UniqueIdentifier,
                                Value = ""
                            };
                        var codeParameter = activityCode != null
                            ? new SqlParameter()
                            {
                                ParameterName = "@Code",
                                SqlDbType = SqlDbType.VarChar,
                                Value = activityCode,
                                Size = 20
                            }
                            : new SqlParameter()
                            {
                                ParameterName = "@Code",
                                SqlDbType = SqlDbType.VarChar,
                                Value = "",
                                Size = 20
                            };
                        var parameters = new SqlParameter[]
                        {
                            userIdParameter, codeParameter, objCreate, objRead, objUpdate, objDelete
                        };
                        string parameterList = string.Join(", ", parameters.Select(p => $"{p.ParameterName}"));
                        string queryString = $"EXEC USP_GetCRUD {parameterList}";
                        var resFromStore = _context.Database.SqlQueryRaw<GetRoleStoredResponse>(queryString, parameters).FirstOrDefault();

                        switch (activity)
                        {
                            case ActivityType.Create:
                                res = resFromStore?.Create ?? false;
                                break;
                            case ActivityType.Read:
                                res = resFromStore?.Read ?? false;
                                break;
                            case ActivityType.Update:
                                res = resFromStore?.Update ?? false;
                                break;
                            case ActivityType.Delete:
                                res = resFromStore?.Delete ?? false;
                                break;
                        }
                    }

                    if (res) return;
                }

                context.Result = new UnauthorizedResult();
                return;
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
                return;
            }*/
        }
    }

    public class GetRoleStoredResponse
    {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
