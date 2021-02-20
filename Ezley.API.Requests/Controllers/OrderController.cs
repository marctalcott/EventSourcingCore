using System;
using System.Threading.Tasks;
using Ezley.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ES.API.Requests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetOrders()
        {
            var command = new GetPendingOrders();
            var accountProjection = await _mediator.Send(command);
            return Ok(accountProjection);
        }
        
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var command = new GetOrder(orderId);
            var accountProjection = await _mediator.Send(command);
            return Ok(accountProjection);
        }
    }
}