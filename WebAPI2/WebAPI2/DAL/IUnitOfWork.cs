using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI2.Models;

namespace WebAPI2.DAL
{
   interface IUnitOfWork
   {
      IRepository<Cliente> ClienteRepository { get; }

      /// <summary>
      /// Commits all changes
      /// </summary>
      void Commit();
      /// <summary>
      /// Discards all changes that has not been commited
      /// </summary>
      void RejectChanges();
      void Dispose();
   }
}
