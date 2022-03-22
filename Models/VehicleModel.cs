namespace TMonitBackend.Models
{
    public class Vehicle
    {
        public long id;
        public long? userId { get; set; }
        public User? user { get; set; }
        public string? brand;
        public string? model;
    }
}