namespace Model.RequestModel;

public class UserRoleRequest
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public string? Description { get; set; }

    public string? Note { get; set; }
}

public class UpdateUserRoleRequest : UserRoleRequest
{
    public Guid Id { get; set; }
}

public class UserRoleIdRequest
{
    public Guid Id { get; set; }
}