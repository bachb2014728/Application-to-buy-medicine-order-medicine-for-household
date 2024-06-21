
namespace backend.Dto.Category
{
    public class CategoryUpdate
    {
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public bool IsEnable { get; set; } = true;
        public int? CategoryParent { get; set;}
        public List<string?> Items { get; set; } = [];
    }
}