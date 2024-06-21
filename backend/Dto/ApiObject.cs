
namespace backend.Dto
{
    public class ApiObject
    {
        public required string Message { get; set; }
        public DateTime Time { get; set; } =  DateTime.UtcNow.AddHours(7);
    }
}