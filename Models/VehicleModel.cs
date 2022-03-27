namespace TMonitBackend.Models
{
    public class Vehicle
    {
        public string Id { get; set; }
        public string? name {get; set;}
        public long? userId { get; set; }
        public User? user { get; set; }
        public string? vehicleId { get; set; }
        public Vehicle? vehicle { get; set; }
        public string? brand { get; set; }
        public string? model { get; set; }
        public uint mileage { get; set; } = 0;
    }
}