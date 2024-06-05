using BLLayer.Interface;
using DALayer.Context;
using DALayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLayer.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MenuDbContext dbContext) : base(dbContext)
        {
        }
    }
}
