using System.Threading;
using System.Threading.Tasks;
using Ezley.Projections;
using Ezley.ProjectionStore;
using MediatR;

namespace Ezley.Requests
{
    public class GetPendingOrders: IRequest<PendingOrdersView>
    {
        // this request needs no properties.
    }
    
    public class GetPendingOrdersHandler : IRequestHandler<GetPendingOrders, PendingOrdersView>
    {
        private IViewRepository _viewRepository;

        public GetPendingOrdersHandler(IViewRepository viewRepository)
        {
            _viewRepository = viewRepository;
        }

        public async Task<PendingOrdersView> Handle(
            GetPendingOrders request,
            CancellationToken cancellationToken)
        {
            var viewName = $"PendingOrdersView";
            var view = await _viewRepository.LoadTypedViewAsync<PendingOrdersView>(viewName);
            return view;
        }
    }
    
}