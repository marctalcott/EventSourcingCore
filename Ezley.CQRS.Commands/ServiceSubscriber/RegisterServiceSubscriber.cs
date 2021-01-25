using System;
using System.Threading;
using System.Threading.Tasks;
using Ezley.Domain.CRM;
using Ezley.Domain.CRM.Repositories;
using Ezley.EventStore;
using Ezley.ValueObjects;
using MediatR;

namespace Ezley.CQRS.Commands
{
    public class RegisterServiceSubscriber:IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }
        public Guid TenantId { get;  }
        public string LegalName { get;  }
        public string DisplayName { get;  }
        public Address Address { get;  }
        public Phone Phone { get;  }
        public Email Email { get;  }

        public RegisterServiceSubscriber(EventUserInfo eventUserInfo,
            Guid id,Guid tenantId, string legalName, string displayName,
            Address address, Phone phone, Email email)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            TenantId = tenantId;
            LegalName = legalName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
        }
    }
    
    public class RegisterServiceSubscriberHandler : IRequestHandler<RegisterServiceSubscriber>
    {
        private ICrmRepository _crmRepository;

        public RegisterServiceSubscriberHandler(ICrmRepository crmRepository)
        {
            _crmRepository = crmRepository;
        }
        
        public async Task<Unit> Handle(
            RegisterServiceSubscriber command,
            CancellationToken cancellationToken)
        {
            // Domain BRs
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");
            if (command.TenantId == Guid.Empty)
                throw new ApplicationException("TenantId is required.");
            
            DateTime postDateUtc = DateTime.UtcNow;
            var serviceSubscriber = new ServiceSubscriber(command.Id,command.TenantId, command.LegalName, command.DisplayName,
                command.Address, command.Phone, command.Email);
            await _crmRepository.SaveServiceSubscriber(command.EventUserInfo, serviceSubscriber);
            return Unit.Value;
        }
    }
}