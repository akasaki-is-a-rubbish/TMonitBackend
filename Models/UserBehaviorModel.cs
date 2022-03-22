using Microsoft.AspNetCore.Identity;

namespace TMonitBackend.Models
{
    public class UserBehavior
    {
        public string id;
        public long userId { get; set; }
        public User? user { get; set; }
        public DateTime dateTime { get; set; }
        public string? imageId { get; set; }
        public UserBehaviorImage? image { get; set; }
        public string description { get; set; } = "abnormal behavior";
        public uint dangerousLevel { get; set; } = 0;
    }
    public class UserBehaviorImage
    {
        public string id { get; set; }
        public byte[] data { get; set; }
    }
}