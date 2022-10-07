namespace TMonitBackend.Models;

public class CourseModel
{
    public string Id { get; set; }

    public string name{ get; set; }

    public CommonImage image{ get; set; }
    public string description{ get; set; }
    public List<User> partationers{ get; set; }
}