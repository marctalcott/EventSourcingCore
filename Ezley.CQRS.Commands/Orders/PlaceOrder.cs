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
    public class PlaceOrder:IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public string OrderName { get; }
        public List<OrderItem> Items { get; }

        public PlaceOrder(EventUserInfo eventUserInfo, Guid id, string orderName, List<OrderItem> items)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            OrderName = orderName;
            Items = items;
        }
    }
    
    public class PlaceOrderHandler : IRequestHandler<PlaceOrder>
    {
        private IOrderSystemRepository _repository;

        public PlaceOrderHandler(IOrderSystemRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Unit> Handle(
            PlaceOrder command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var order = new Order(command.Id, command.OrderName, command.Items);
            var saved = await _repository.SaveOrder(command.EventUserInfo, order);
           
            if (!saved)
            {
                throw new ApplicationException("Failed to save Tenant");
            }
            return Unit.Value;
        }
    }
}