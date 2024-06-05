using BLLayer.Interface;
using DALayer.Context;
using DALayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLayer.Repository
{
    public class MenuRepository : GenericRepository<MenuItem>, IMenuRepository
    {
        private readonly MenuDbContext _dbContext;

        public MenuRepository(MenuDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MenuItem>> GetEmployeeByNameAsync(string searchValue)
        {
          return await _dbContext.MenuItems.Where(i=>i.Name.ToLower().Contains(searchValue.ToLower())).ToListAsync();
        }
    }
}
