using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.BackgroundServices;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IBus bus;

        public RabbitMqController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpPost]
        public async Task<string> Post([FromQuery] UserRequest request)
        {
            var response = await bus.Rpc.RequestAsync<UserRequest, UserResponse>(request);

            return response.Response;
        }

        [HttpPut]
        public async Task<string> Put([FromQuery] UserRequest request)
        {
            var response = await bus.Rpc.RequestAsync<UserRequest, UserResponse>(request);

            return response.Response;
        }

        [HttpDelete]
        public async Task<string> Delete([FromQuery] UserRequest request)
        {
            var response = await bus.Rpc.RequestAsync<UserRequest, UserResponse>(request);

            return response.Response;
        }
    }
}
