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
    public class RegisterTenant:IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }
        public string LegalName { get;  }
        public string DisplayName { get;  }
        public Address Address { get;  }
        public Phone Phone { get;  }
        public Email Email { get;  }

        public RegisterTenant(EventUserInfo eventUserInfo,
            Guid id,string legalName, string displayName,
            Address address, Phone phone, Email email)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            LegalName = legalName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
        }
    }
    
    public class RegisterTenantHandler : IRequestHandler<RegisterTenant>
    {
        private ICrmRepository _crmRepository;

        public RegisterTenantHandler(ICrmRepository crmRepository)
        {
            _crmRepository = crmRepository;
        }
        
        public async Task<Unit> Handle(
            RegisterTenant command,
            CancellationToken cancellationToken)
        {
            // Domain BRs
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");
           
            DateTime postDateUtc = DateTime.UtcNow;
            var Tenant = new Tenant(command.Id, command.LegalName, command.DisplayName,
                command.Address, command.Phone, command.Email);
            
            var saved = await _crmRepository.SaveTenant(command.EventUserInfo, Tenant);
            if (!saved)
            {
                throw new ApplicationException("Failed to save Tenant");
            }
            return Unit.Value;
        }
    }
}