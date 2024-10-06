namespace Model.ResponseModel;

public class UserRoleResponse
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedUser { get; set; }
    public string? CreatedName { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedUser { get; set; }
    public string? UpdatedName { get; set; }
}