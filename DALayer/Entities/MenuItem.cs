
using Microsoft.AspNetCore.Http;

namespace DALayer.Entities
{
    public class MenuItem :BaseEntity
    {
       
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

       public string? ImageUrl { get; set; } = null!;
       
        public bool IsActive { get; set; }=true;

    }
}
