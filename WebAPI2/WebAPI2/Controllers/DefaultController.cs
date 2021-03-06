﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;
using System.Data.SqlClient;
using WebAPI2.DAL;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebAPI2.Controllers
{
   [RoutePrefix("api/meuprojeto")]
   public class DefaultController : ApiController
    {

      public string ConnectionString = "Data Source = DESK30; Initial Catalog = webAPI; Integrated Security = True";
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
                        Data_Nascimento = reader["data_nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["data_nascimento"]),
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
                        Data_Nascimento = reader["data_nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["data_nascimento"]),
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

      [HttpDelete]
      [Route("cliente/{id:int}")]
      public HttpResponseMessage DeleteById(int id)
      {
         
         MyDbContext dbContext = new MyDbContext("Data Source=DESK30;Initial Catalog=webAPI;Integrated Security=True");
         var unitOfWork = new UnitOfWork(dbContext);


         // Delete
         var cliente = unitOfWork.ClienteRepository.Entities
             .First(n => n.Id == id);

            unitOfWork.ClienteRepository.Remove(cliente);
            unitOfWork.Commit(); // Save changes to database

         /*
         try
         {
            bool resultado = false;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
               connection.Open();

               using (SqlCommand command = new SqlCommand())
               {
                  command.Connection = connection;
                  command.CommandText = "delete from clientes where id = @id";
                  command.Parameters.AddWithValue("id", id);

                  int i = command.ExecuteNonQuery();
                  resultado = i > 0;
               }

               connection.Close();
            }

            return Request.CreateResponse(HttpStatusCode.OK, resultado);
         }
         catch (Exception ex)
         {
            return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
         }*/

         return Request.CreateResponse(HttpStatusCode.OK, true);
      }

      [HttpPost]
      [Route("cliente")]
      public HttpResponseMessage Post(Cliente cliente)
      {
         try
         {
            bool resultado = false;

            if (cliente == null) throw new ArgumentNullException("cliente");

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
               connection.Open();

               using (SqlCommand command = new SqlCommand())
               {
                  command.Connection = connection;
                  command.CommandText = "insert into clientes(nome, data_nascimento, email) values(@nome, @data_nascimento, @email)";

                  command.Parameters.AddWithValue("nome", cliente.Nome);
                  command.Parameters.AddWithValue("data_nascimento", DateTime.Now);
                  command.Parameters.AddWithValue("email", cliente.Email);

                  int i = command.ExecuteNonQuery();
                  resultado = i > 0;
               }

               connection.Close();
            }

            return Request.CreateResponse(HttpStatusCode.OK, resultado);
         }
         catch (Exception ex)
         {
            return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
         }
      }


      [HttpPut]
      [Route("cliente/{id:int}")]
      public HttpResponseMessage Put(int id, Cliente cliente)
      {
         try
         {
            bool resultado = false;

            if (cliente == null) throw new ArgumentNullException("cliente");
            if (id == 0) throw new ArgumentNullException("id");

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
               connection.Open();

               using (SqlCommand command = new SqlCommand())
               {
                  command.Connection = connection;
                  command.CommandText = "update clientes set nome = @nome, data_nascimento = @data_nascimento, email = @email where id = @id";

                  command.Parameters.AddWithValue("id", id);
                  command.Parameters.AddWithValue("nome", cliente.Nome);
                  command.Parameters.AddWithValue("data_nascimento", cliente.Data_Nascimento);
                  command.Parameters.AddWithValue("email", cliente.Email);

                  int i = command.ExecuteNonQuery();
                  resultado = i > 0;
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


      [HttpGet]
      [Route("cliente/{email}")]
      public HttpResponseMessage GetByEmail(string email)
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
                  command.CommandText = "select id, nome, data_nascimento, email from clientes where email = @email";
                  command.Parameters.AddWithValue("email", email);

                  SqlDataReader reader = command.ExecuteReader();

                  while (reader.Read())
                  {
                     cliente = new Cliente()
                     {
                        Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                        Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                        Data_Nascimento = reader["data_nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["data_nascimento"]),
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
