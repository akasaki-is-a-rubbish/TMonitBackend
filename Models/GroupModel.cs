namespace TMonitBackend.Models;

public class GroupModel
{
    public string Id { get; set; }

    public string description { get; set; } = "user group";

    List<User> users { get; set; }
}