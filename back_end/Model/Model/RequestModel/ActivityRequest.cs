namespace Model.RequestModel;

public class ActivityRequest
{
    public string? Code { get; set; }

    public string? ActivityName { get; set; }

    public string? Description { get; set; }

    public string? ApplicationName { get; set; }

    public string? Note { get; set; }
}

public class UpdateActivityRequest : ActivityRequest
{
    public Guid Id { get; set; }
}

public class ActivityIdRequest
{
    public Guid Id { get; set; }
}