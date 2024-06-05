using DALayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLayer.Interface
{
    public interface IMenuRepository :IGenericRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetEmployeeByNameAsync(string searchValue);
    }
}
