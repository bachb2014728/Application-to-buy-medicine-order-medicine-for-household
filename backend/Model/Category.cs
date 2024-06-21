
using System.ComponentModel.DataAnnotations.Schema;
using backend.Dto.Category;

namespace backend.Model
{
    [Table("Categories")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public bool IsEnable { get; set; } = true;
        public int? CategoryParent { get; set;}
        public List<Product>? Products { get; set; } = [];

        public CategoryDto ToCategoryDto()
        {
            return new CategoryDto
            {
                Id = this.Id,
                Name = this.Name,
                URL = this.URL,
                CategoryParent = this.CategoryParent,
            };
        }
    }
}