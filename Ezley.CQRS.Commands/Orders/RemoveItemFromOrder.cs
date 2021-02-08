using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ezley.EventStore;
using Ezley.OrderSystem;
using Ezley.OrderSystem.Repositories;
using Ezley.ValueObjects;
using MediatR;

namespace Ezley.Commands
{
    public class RemoveItemFromOrder:IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public OrderItem Item { get; }

        public RemoveItemFromOrder(EventUserInfo eventUserInfo, Guid id, OrderItem item)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Item = item;
        }
    }
    
    public class RemoveItemFromOrderHandler : IRequestHandler<RemoveItemFromOrder>
    {
        private IOrderSystemRepository _repository;

        public RemoveItemFromOrderHandler(IOrderSystemRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Unit> Handle(
            RemoveItemFromOrder command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var order = await _repository.LoadOrder(command.Id); 
            order.RemoveItem(command.Item);
            var saved = await _repository.SaveOrder(command.EventUserInfo, order);
           
            if (!saved)
            {
                throw new ApplicationException("Failed to save.");
            }
            return Unit.Value;
        }
    }
}