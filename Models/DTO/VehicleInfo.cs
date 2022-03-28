namespace TMonitBackend.Models.DTO{
    public class VehicleInfo{
        public string Id { get; set; }
        public string? name { get; set; }
        public string? brand { get; set; }
        public string? model { get; set; }
        public uint mileage { get; set; } = 0;
        public string? image { get; set; }
    }
}