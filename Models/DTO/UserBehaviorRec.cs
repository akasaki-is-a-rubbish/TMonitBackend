namespace TMonitBackend.Models.DTO{
    public class UserBehaviorRec{
        public string userId { get; set; }
        public long vehicleId { get; set; }
        public string description { get; set; }
        DateTime dateTime { get; set; } = DateTime.Now;
    }
}