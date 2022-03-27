namespace TMonitBackend.Models.DTO
{
    public class UserBehaviorRec
    {
        public string? id { get; set; }
        public string vehicleIdEncrypted { get; set; }
        public string description { get; set; } = "dangerous behavior";
        public DateTime dateTime { get; set; } = DateTime.Now;
        public string? image { get; set; }
        public uint dangerousLevel { get; set; } = 0;
    }
}