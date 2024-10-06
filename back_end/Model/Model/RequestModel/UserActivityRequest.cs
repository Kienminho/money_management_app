namespace Model.RequestModel;

public class UserActivityRequest
{
    public Guid? ActivityId { get; set; }
    public bool? C { get; set; }
    public bool? R { get; set; }
    public bool? U { get; set; }
    public bool? D { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
}

public class UpdateUserActivityRequest
{
    public Guid UserId { get; set; }
    public List<UserActivityRequest> UserActivityRequests { get; set; } = new List<UserActivityRequest>();
}