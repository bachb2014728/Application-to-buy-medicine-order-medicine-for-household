
namespace backend.Dto.Product.DosageForm
{
    public class DosageFormDto
    {
        public int Id { get; set;}
        public string? Name { get; set; }
        public string? Info { get; set; }
        public List<string> Products = [];
    }
}