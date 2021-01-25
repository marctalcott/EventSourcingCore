using System;
using System.Threading;
using System.Threading.Tasks;
using Ezley.Domain.CRM.Repositories;
using Ezley.EventStore;
using Ezley.ValueObjects;
using MediatR;

namespace Ezley.CQRS.Commands
{
    public class EditTenant: IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }
        public Editable<string> LegalName { get;  }
        public Editable<string> DisplayName { get;  }
        public Editable<Address> Address { get;  }
        public Editable<Phone> Phone { get;  }
        public Editable<Email> Email { get;  }

        public EditTenant(EventUserInfo eventUserInfo,
            Guid id, Editable<string> legalName,
            Editable<string> displayName, Editable<Address> address,
            Editable<Phone> phone, Editable<Email> email)
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

    public class EditTenantHandler : IRequestHandler<EditTenant>
    {
        private ICrmRepository _crmRepository;

        public EditTenantHandler(ICrmRepository crmRepository)
        {
            _crmRepository = crmRepository;
        }
        
        public async Task<Unit> Handle(
            EditTenant command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");
            
            var Tenant = await _crmRepository.LoadTenant(command.Id);

            if (command.LegalName.Edit)
            {
                Tenant.ChangeLegalName(command.LegalName.Value);
            }

            if (command.DisplayName.Edit)
            {
                Tenant.ChangeDisplayName(command.DisplayName.Value);
            }

            if (command.Address.Edit)
            {
                Tenant.ChangeAddress(command.Address.Value);
            }

            if (command.Email.Edit)
            {
                Tenant.ChangeEmail(command.Email.Value);
            }

            if (command.Phone.Edit)
            {
                Tenant.ChangePhone(command.Phone.Value);
            }

            await _crmRepository.SaveTenant(command.EventUserInfo, Tenant);
            return Unit.Value;
        }
    }
}