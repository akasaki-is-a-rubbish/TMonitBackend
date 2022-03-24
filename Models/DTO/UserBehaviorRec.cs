namespace TMonitBackend.Models.DTO{
    public class UserBehaviorRec{
        public long? userId { get; set; }
        public long vehicleId { get; set; }
        public string description { get; set; } = "dangerous behavior";
        DateTime dateTime { get; set; } = DateTime.Now;
    }
}