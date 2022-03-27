using Microsoft.AspNetCore.Identity;

namespace TMonitBackend.Models{
    public class User : IdentityUser<long> {
        public List<UserBehavior>? bhRecs { get; set; }
        public List<Vehicle>? vehicles { get; set; }
        public string? imageId { get; set; }
        public CommonImage? image { get; set; }
        public string? EmergencyContract { get; set; }
    }
}