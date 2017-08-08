using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebAPI2.Models;

namespace WebAPI2.DAL
{
   public class MyDbContext : DbContext
   {
      public virtual DbSet<Cliente> Clientes { get; set; }
      public MyDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
      {
      }

   }
}