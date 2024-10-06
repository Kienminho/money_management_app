namespace Model.ResponseModel;

public class RoleActivityResponse
{
    public Guid Id { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public string? Code { get; set; }
    public bool? C { get; set; }
    public bool? R { get; set; }
    public bool? U { get; set; }
    public bool? D { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedUser { get; set; }
    public string? CreatedName { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedUser { get; set; }
    public string? UpdatedName { get; set; }
}