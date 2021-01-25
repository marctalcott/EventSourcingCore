using System;
using System.Threading.Tasks;
using Ezley.API.Commands.ViewModels;
using Ezley.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ezley.API.Commands.Controllers
{  
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ServiceSubscriberController : BaseController
    { 
        private readonly IMediator _mediator;

        public ServiceSubscriberController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterServiceSubscriberViewModel vm)
        {
            var userInfo = GetUserInfo();
            var command = new RegisterServiceSubscriber(userInfo, vm.Id, vm.TenantId,
                vm.LegalName, vm.DisplayName, vm.Address, vm.Phone, vm.Email);
            
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit(Guid id,
            [FromBody] EditServiceSubscriberViewModel vm)
        {
            var userInfo = base.GetUserInfo();
            var command = new EditServiceSubscriber(userInfo, id, vm.LegalName, vm.DisplayName, vm.Address,
                vm.Phone, vm.Email,vm.Active);
                
            await _mediator.Send(command);
            return Ok();
        }
    }
}