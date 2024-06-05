using BLLayer.Interface;
using DALayer.Context;
using DALayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLayer.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MenuDbContext _dbContext;
        Hashtable _repository;
        public IMenuRepository MenuRepository { get; set; }
        public ICategoryRepository CategoryRepository { get ; set; }

        public UnitOfWork(MenuDbContext dbContext) 
        {
          
           MenuRepository=new MenuRepository(dbContext);
           CategoryRepository=new CategoryRepository(dbContext);   
            _dbContext = dbContext;
        }
        public async Task<int> CompleteAsync()
         =>await _dbContext.SaveChangesAsync(); 
              
        
        public void Dispose()
        {
            _dbContext.Dispose();
        }

     

    }
}
