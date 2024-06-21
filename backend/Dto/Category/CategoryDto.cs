
namespace backend.Dto.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public bool IsEnable { get; set; } = true;
        public int? CategoryParent { get; set;}
        public List<CategoryItem?> Items { get; set; } = [];
    }
}