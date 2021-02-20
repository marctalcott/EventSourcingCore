using System;
using System.Threading;
using System.Threading.Tasks;
using Ezley.Projections;
using Ezley.ProjectionStore;
using MediatR;

namespace Ezley.Requests
{
    public class GetOrder: IRequest<OrderView>
    {
        public Guid Id { get; }

        public GetOrder(Guid id)
        {
            Id = id;
        }
    }
    
    public class GetOrderHandler : IRequestHandler<GetOrder, OrderView>
    {
        private IViewRepository _viewRepository;

        public GetOrderHandler(IViewRepository viewRepository)
        {
            _viewRepository = viewRepository;
        }

        public async Task<OrderView> Handle(
            GetOrder request,
            CancellationToken cancellationToken)
        {
            var viewName = $"OrderView:{request.Id}";
            var view = await _viewRepository.LoadTypedViewAsync<OrderView>(viewName);
            return view;
        }
    }
    
}