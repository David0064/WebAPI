using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public TestController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            string query = @"select * from Test01 where id = @id";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("MariaDb");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand mycommand = new MySqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@id", id);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpGet]

        public IActionResult Get([FromQuery] int? page)
        {
            string query = @"select * from Test01";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("MariaDb");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand mycommand = new MySqlCommand(query, mycon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            List<Test01> listTest01 = new List<Test01>();

            listTest01 = (from DataRow dr in table.Rows
                          select new Test01
                          {
                              Id = Convert.ToInt32(dr["Id"]),
                              Nama = dr["Nama"].ToString(),
                              Created = Convert.ToDateTime(dr["Created"]),
                              Updated = Convert.ToDateTime(dr["Updated"])
                          }).ToList();
            int pageSize = 20;
            listTest01 = listTest01.Skip(pageSize * ((page ?? 1) - 1) ).Take(pageSize).ToList();

            return new JsonResult(listTest01);
        }

        [HttpPost("create")]

        public IActionResult Create(Test01 model)
        {
            string query = @"insert into Test01 (Nama, Status, Created, Updated)
                VALUES (@nama, @status, @created, @updated)";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("MariaDb");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand mycommand = new MySqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@nama", model.Nama);
                    mycommand.Parameters.AddWithValue("@status", model.Status);
                    mycommand.Parameters.AddWithValue("@created", DateTime.Now);
                    mycommand.Parameters.AddWithValue("@updated", DateTime.Now);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut("update")]

        public IActionResult Update(Test01 model)
        {
            string query = @"update Test01 
                set Nama = @nama, Updated = @updated 
                where id = @id";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("MariaDb");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand mycommand = new MySqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@nama", model.Nama);
                    mycommand.Parameters.AddWithValue("@id", model.Id);
                    mycommand.Parameters.AddWithValue("@updated", DateTime.Now);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete("delete/{id}")]

        public JsonResult Delete(int id)
        {
            string query = @"delete from Test01 where id = @id";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("MariaDb");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand mycommand = new MySqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@id", id);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
