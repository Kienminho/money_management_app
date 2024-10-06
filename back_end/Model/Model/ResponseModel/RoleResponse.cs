namespace Model.ResponseModel;

public class RoleResponse
{
    public Guid Id { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public int? RoleType { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedUser { get; set; }
    public string? CreatedName { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedUser { get; set; }
    public string? UpdatedName { get; set; }
}