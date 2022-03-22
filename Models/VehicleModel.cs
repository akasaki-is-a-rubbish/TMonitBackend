namespace TMonitBackend.Models
{
    public class Vehicle
    {
        public string id;
        public long? userId { get; set; }
        public User? user { get; set; }
    }
}