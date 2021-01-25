using System;
using System.Threading;
using System.Threading.Tasks;
using Ezley.Domain.CRM.Repositories;
using Ezley.EventStore;
using MediatR;

namespace Ezley.CQRS.Commands
{
    public class DeactivateServiceSubscriber: IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }

        public DeactivateServiceSubscriber(EventUserInfo eventUserInfo, Guid id)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
        }
    }

    public class DeactivateServiceSubscriberHandler : IRequestHandler<DeactivateServiceSubscriber>
    {
        private ICrmRepository _crmRepository;

        public DeactivateServiceSubscriberHandler(ICrmRepository crmRepository)
        {
            _crmRepository = crmRepository;
        }
        
        public async Task<Unit> Handle(
            DeactivateServiceSubscriber command,
            CancellationToken cancellationToken)
        { 
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");
            
            var Tenant = await _crmRepository.LoadTenant(command.Id);

            Tenant.Deactivate();
            await _crmRepository.SaveTenant(command.EventUserInfo, Tenant);
            return Unit.Value;
        }
    }
}