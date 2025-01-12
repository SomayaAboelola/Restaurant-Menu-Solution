﻿using DALayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLayer.Interface
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();    
        Task<T> GetAsync(int id);
        Task AddAsync (T entity);
        void Update (T entity);
        void Delete (T entity);

    }
}
