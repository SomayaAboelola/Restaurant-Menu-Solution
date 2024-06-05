using BLLayer.Interface;
using DALayer.Context;
using DALayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BLLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MenuDbContext _dbContext;

        public GenericRepository(MenuDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await  _dbContext.Set<T>().ToListAsync();
        }

 



        public async Task<T> GetAsync(int id)
       => await _dbContext.Set<T>().FindAsync(id);


        public async Task AddAsync(T entity)
        {
           await _dbContext.AddAsync(entity);
           
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
           
        }
        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
            
        }

      
    }
}
