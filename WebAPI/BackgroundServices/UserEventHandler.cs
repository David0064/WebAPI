using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.BackgroundServices
{
    public class UserRequest
    {
        public string command { get; set; }
        public Test01 data { get; set; }
    }

    public class UserResponse
    {
        public string Response { get; set; }
        public UserResponse() { }
    }

    public class UserEventHandler : BackgroundService
    {
        private readonly IBus bus;
        private readonly IConfiguration configuration;

        public UserEventHandler(IBus bus, IConfiguration configuration)
        {
            this.bus = bus;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await bus.Rpc.RespondAsync<UserRequest, UserResponse>(ProcessQueueRequest);
        }

        private UserResponse ProcessQueueRequest(UserRequest userRequest)
        {
            if(userRequest.command == "create")
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
                        mycommand.Parameters.AddWithValue("@nama", userRequest.data.Nama);
                        mycommand.Parameters.AddWithValue("@status", userRequest.data.Status);
                        mycommand.Parameters.AddWithValue("@created", DateTime.Now);
                        mycommand.Parameters.AddWithValue("@updated", DateTime.Now);
                        myReader = mycommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    }
                }
            }
            else if(userRequest.command == "update")
            {
                string query = @"update Test01 
                set Status = @status, Updated = @updated 
                where nama = @nama";

                DataTable table = new DataTable();
                string sqlDataSource = configuration.GetConnectionString("MariaDb");
                MySqlDataReader myReader;
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand mycommand = new MySqlCommand(query, mycon))
                    {
                        mycommand.Parameters.AddWithValue("@nama", userRequest.data.Nama);
                        mycommand.Parameters.AddWithValue("@status", userRequest.data.Status);
                        mycommand.Parameters.AddWithValue("@updated", DateTime.Now);
                        myReader = mycommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    }
                }
            }
            else if(userRequest.command == "delete")
            {
                string query = @"delete from Test01 where nama = @nama";

                DataTable table = new DataTable();
                string sqlDataSource = configuration.GetConnectionString("MariaDb");
                MySqlDataReader myReader;
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand mycommand = new MySqlCommand(query, mycon))
                    {
                        mycommand.Parameters.AddWithValue("@nama", userRequest.data.Nama);
                        myReader = mycommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    }
                }
            }
            return new UserResponse() { Response = $"Successfully {userRequest.command}" };
        }
    }
}
