using System;
using System.Threading.Tasks;
using AutoMapper;
using Ezley.API.Commands.ViewModels;
using Ezley.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ezley.API.Commands.Controllers
{  
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TenantController : BaseController
    { 
        private readonly IMediator _mediator;

        public TenantController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterTenantViewModel vm)
        {
            var userInfo = GetUserInfo();
            var command = new RegisterTenant(userInfo, vm.Id, vm.LegalName, vm.DisplayName,
                vm.Address, vm.Phone, vm.Email);
            
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit(Guid id,
            [FromBody] EditTenantViewModel vm)
        {
            var userInfo = GetUserInfo();
            var command = new EditTenant(userInfo, id, vm.LegalName,
                vm.DisplayName, vm.Address, vm.Phone, vm.Email);
            
            await _mediator.Send(command);
            return Ok();
        }
    }
}