
namespace backend.Dto.Store
{
    public class StoreCreate
    {
        public required string Name { get; set; }
        public required string URL { get; set; }
        public string? Info { get; set;}
        public required string Address { get; set; }
    }
}