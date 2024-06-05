using DALayer.Entities;

namespace PLayer.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();
    }
}
