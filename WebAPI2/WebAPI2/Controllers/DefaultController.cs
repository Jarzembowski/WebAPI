using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;
using System.Data.SqlClient;

namespace WebAPI2.Controllers
{
   [RoutePrefix("api/meuprojeto")]
   public class DefaultController : ApiController
    {
      private string ConnectionString = "Data Source=DESK30;Initial Catalog=webAPI;Integrated Security=True";

      [HttpGet]
      [Route("datahora/consulta")]
      public HttpResponseMessage GetDataHoraServidor()
      {
         try
         {
            var dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            return Request.CreateResponse(HttpStatusCode.OK, dataHora);
         }
         catch (Exception ex)
         {
            return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
         }
      }

      [HttpPost]
      [Route("cadastrar")]
      public HttpResponseMessage PostCadastro(Cliente cliente)
      {
         try
         {
            return Request.CreateResponse(HttpStatusCode.OK, "Cadastro do usuário " + cliente.Nome + " realizado.");
         }
         catch (Exception ex)
         {

            return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
         }
      }

      [HttpGet]
      [Route("clientes/todos")]
      public HttpResponseMessage GetAll()
      {
         try
         {
            List<Cliente> lstClientes = new List<Cliente>();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
               connection.Open();

               using (SqlCommand command = new SqlCommand())
               {
                  command.Connection = connection;
                  command.CommandText = "select id, nome, data_nascimento, email from clientes";

                  SqlDataReader reader = command.ExecuteReader();

                  while (reader.Read())
                  {
                     Cliente cliente = new Cliente()
                     {
                        Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                        Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                        DataNascimento = reader["data_nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["data_nascimento"]),
                        Email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString()
                     };

                     lstClientes.Add(cliente);
                  }
               }

               connection.Close();
            }

            return Request.CreateResponse(HttpStatusCode.OK, lstClientes.ToArray());
         }
         catch (Exception ex)
         {
            return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
         }
      }

      [HttpGet]
      [Route("cliente/{id:int}")]
      public HttpResponseMessage GetById(int id)
      {
         try
         {
            Cliente cliente = null;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
               connection.Open();

               using (SqlCommand command = new SqlCommand())
               {
                  command.Connection = connection;
                  command.CommandText = "select id, nome, data_nascimento, email from clientes where id = @id";
                  command.Parameters.AddWithValue("id", id);

                  SqlDataReader reader = command.ExecuteReader();

                  while (reader.Read())
                  {
                     cliente = new Cliente()
                     {
                        Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                        Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                        DataNascimento = reader["data_nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["data_nascimento"]),
                        Email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString()
                     };
                  }
               }

               connection.Close();
            }

            return Request.CreateResponse(HttpStatusCode.OK, cliente);
         }
         catch (Exception ex)
         {
            return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
         }
      }



   }
}
