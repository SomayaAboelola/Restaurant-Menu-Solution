using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.Entities
{
    public class Category :BaseEntity
    {
       public string Name { get; set; } = null!;
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();
    }
}
