using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
   public class Cliente
   {
      public int Id { get; set; }
      public string Nome { get; set; }
      public DateTime Data_Nascimento { get; set; }
      public string Email { get; set; }
   }

}