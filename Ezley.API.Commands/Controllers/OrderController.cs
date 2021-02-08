using System.Threading.Tasks;
using Ezley.API.Commands.ViewModels;
using Ezley.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ezley.API.Commands.Controllers
{  
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : BaseController
    { 
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PlaceOrder(
            [FromBody] PlaceOrderViewModel vm)
        {
            var userInfo = GetUserInfo();
            var command = new PlaceOrder(userInfo, vm.Id, vm.OrderName, vm.Items);
            
            await _mediator.Send(command);
            return Ok();
        }
    }
}