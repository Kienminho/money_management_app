namespace Model.ResponseModel;

public class ActivityResponse
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? ActivityName { get; set; }
    public string? Description { get; set; }
    public string? ApplicationName { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedUser { get; set; }
    public string? CreatedName { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedUser { get; set; }
    public string? UpdatedName { get; set; }
}