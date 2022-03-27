using EasyNetQ;
using Microsoft.Extensions.Hosting;
using System;
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

        public UserEventHandler(IBus bus)
        {
            this.bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await bus.Rpc.RespondAsync<UserRequest, UserResponse>(ProcessQueueRequest);
        }

        private UserResponse ProcessQueueRequest(UserRequest userRequest)
        {
            if(userRequest.command == "create")
            {

            }
            else if(userRequest.command == "update")
            {

            }
            else if(userRequest.command == "delete")
            {

            }
            return new UserResponse() { Response = $"Successfully {userRequest.command}" };
        }
    }
}
