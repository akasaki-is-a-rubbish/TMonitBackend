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
    }
    public class UserBehaviorImage
    {
        public string id { get; set; }
        public byte[] data { get; set; }
    }
}