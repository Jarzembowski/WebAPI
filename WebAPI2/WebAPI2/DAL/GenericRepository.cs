using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebAPI2.DAL
{
   public class GenericRepository<T> : IRepository<T> where T : class
   {
      private readonly MyDbContext _dbContext;
      private IDbSet<T> _dbSet => _dbContext.Set<T>();
      public IQueryable<T> Entities => _dbSet;

      public GenericRepository(MyDbContext dbContext)
      {
         _dbContext = dbContext;
      }

      public void Remove(T entity)
      {
         _dbSet.Remove(entity);
      }
      public void Add(T entity)
      {
         _dbSet.Add(entity);
      }
   }
}