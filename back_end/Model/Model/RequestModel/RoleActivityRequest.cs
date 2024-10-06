namespace Model.RequestModel;

public class RoleActivityRequest
{
    public Guid? ActivityId { get; set; }

    public bool? C { get; set; }

    public bool? R { get; set; }

    public bool? U { get; set; }

    public bool? D { get; set; }

    public string? Description { get; set; }

    public string? Note { get; set; }
}

public class UpdateRoleActivityRequest
{
    public Guid RoleId { get; set; }
    public List<RoleActivityRequest> RoleActivityRequests { get; set; } = new List<RoleActivityRequest>();
}