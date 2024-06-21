
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Dto.Product
{
    public class CreateProductRequest
    {
        public required string Name { get; set; }
        public required string URL { get; set; }
        public required int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Sale { get; set; }
        public required string Content { get; set; }

        public required List<int> Uses { get; set; } = [];
        public required int ManufacturerId { get; set; }
        public required List<int> Contraindications { get; set; } = [];
        public required List<int> DosageForms { get; set; } = [];
        public bool? Status { get; set; }
        public List<int>? Categories { get; set; } = [];

        public List<int>? Images { get; set; } = [];
        public int StoreId { get; set; }
    }
}