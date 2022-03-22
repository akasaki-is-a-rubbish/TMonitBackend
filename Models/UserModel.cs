using Microsoft.AspNetCore.Identity;

namespace TMonitBackend.Models{
    public class User : IdentityUser<long> {
        public List<UserBehavior>? bhRecs { get; set; }
        public List<Vehicle>? vehicles { get; set; }
    }
}