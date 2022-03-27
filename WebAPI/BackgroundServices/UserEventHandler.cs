using EasyNetQ;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.BackgroundServices
{
    public class UserRequest
    {
        public long Id { get; set; }
        public UserRequest(long id)
        {
            Id = id;
        }
    }

    public class UserResponse
    {
        public string Name { get; set; }
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
            await bus.Rpc.RespondAsync<UserRequest, UserResponse>(ProcessUserRequest);
        }

        private UserResponse ProcessUserRequest(UserRequest userRequest)
        {
            return new UserResponse() { Name = "Ipsum" };
        }
    }
}
