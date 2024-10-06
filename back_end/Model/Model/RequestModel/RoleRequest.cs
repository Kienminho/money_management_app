namespace Model.RequestModel;

public class RoleRequest
{
    public string? RoleName { get; set; }

    public string? Description { get; set; }

    public string? Note { get; set; }

    public int? RoleType { get; set; }
}

public class UpdateRoleRequest : RoleRequest
{
    public Guid Id { get; set; }
}

public class RoleIdRequest
{
    public Guid Id { get; set; }
}