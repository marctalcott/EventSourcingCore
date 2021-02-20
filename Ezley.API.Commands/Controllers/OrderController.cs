using System;
using System.Threading.Tasks;
using Ezley.API.Commands.ViewModels;
using Ezley.Commands;
using Ezley.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ezley.API.Commands.Controllers
{
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
        
        [HttpPut]
        [Route("{orderId}/Add")]
        public async Task<IActionResult> AddToOrder(Guid orderId,
            [FromBody] AddItemToOrderViewModel vm)
        {
            var userInfo = GetUserInfo();

            var command = new AddItemToOrder(userInfo, orderId, new OrderItem(vm.TimeAdded, vm.Name, vm.Amount));
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPut]
        [Route("{orderId}/Remove")]
        public async Task<IActionResult> RemoveFromOrder(Guid orderId,
            [FromBody] RemoveItemFromOrderViewModel vm)
        {
            var userInfo = GetUserInfo();

            var command = new RemoveItemFromOrder(userInfo, orderId, vm.Name);
            await _mediator.Send(command);
            return Ok();
        }
    }
}