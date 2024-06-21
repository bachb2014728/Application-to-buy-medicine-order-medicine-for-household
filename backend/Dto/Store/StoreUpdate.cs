
namespace backend.Dto.Store
{
    public class StoreUpdate
    {
        public string? Name { get; set; } 
        public string? URL { get; set; }
        public string? Info { get; set;}
        public List<string?> Followers { get; set; } = [];
        public string? Address { get; set; }
    }
}