
namespace TMonitBackend.Models
{
    public class UserBehavior
    {
        public string Id { get; set; }
        public long userId { get; set; }
        public User? user { get; set; }
        public string? vehicleId { get; set; }
        public Vehicle? vehicle { get; set; }
        public DateTime dateTime { get; set; }
        public string? imageId { get; set; }
        public CommonImage? image { get; set; }
        public string description { get; set; } = "abnormal behavior";
        public uint dangerousLevel { get; set; } = 0;
    }
}