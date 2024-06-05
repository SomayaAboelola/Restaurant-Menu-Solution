using DALayer.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLayer.Models
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }
        [MinLength(5, ErrorMessage = "min Name 5")]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public IFormFile? Image { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
