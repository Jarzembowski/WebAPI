using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI2.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebAPI2.DAL
{
   public class UnitOfWork : IUnitOfWork
   {
      private readonly MyDbContext _dbContext;

      public IRepository<Cliente> ClienteRepository =>
       new GenericRepository<Cliente>(_dbContext);

      public UnitOfWork(MyDbContext dbContext)
      {
         _dbContext = dbContext;
      }
      public void Commit()
      {
         _dbContext.SaveChanges();
      }
      public void Dispose()
      {
         _dbContext.Dispose();
      }

      public void RejectChanges()
      {
         foreach (var entry in _dbContext.ChangeTracker.Entries()
               .Where(e => e.State != EntityState.Unchanged))
         {
            switch (entry.State)
            {
               case EntityState.Added:
                  entry.State = EntityState.Detached;
                  break;
               case EntityState.Modified:
               case EntityState.Deleted:
                  entry.Reload();
                  break;
            }
         }
      }

   }
}