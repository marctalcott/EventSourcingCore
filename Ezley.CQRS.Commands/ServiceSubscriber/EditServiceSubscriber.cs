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
    public class EditServiceSubscriber: IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }
        public Editable<string> LegalName { get;  }
        public Editable<string> DisplayName { get;  }
        public Editable<Address> Address { get;  }
        public Editable<Phone> Phone { get;  }
        public Editable<Email> Email { get;  }
        public Editable<bool> Active { get;  }

        public EditServiceSubscriber(EventUserInfo eventUserInfo,
            Guid id, Editable<string> legalName,
            Editable<string> displayName, Editable<Address> address,
            Editable<Phone> phone, Editable<Email> email, Editable<bool> active)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            LegalName = legalName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
        }
    }

    public class EditServiceSubscriberHandler : IRequestHandler<EditServiceSubscriber>
    {
        private ICrmRepository _crmRepository;

        public EditServiceSubscriberHandler(ICrmRepository crmRepository)
        {
            _crmRepository = crmRepository;
        }
        
        public async Task<Unit> Handle(
            EditServiceSubscriber command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");
            
            var serviceSubscriber = await _crmRepository.LoadServiceSubscriber(command.Id);

            HandleLegalNameChange(command, serviceSubscriber);
            HandleDisplayNameChange(command, serviceSubscriber);
            HandleAddressChange(command, serviceSubscriber);
            HandleEmailChange(command, serviceSubscriber);
            HandlePhoneChange(command, serviceSubscriber);
            HandleActiveStatusChange(command, serviceSubscriber);
            
            await _crmRepository.SaveServiceSubscriber(command.EventUserInfo, serviceSubscriber);
            return Unit.Value;
        }

        private static void HandleLegalNameChange(EditServiceSubscriber command, ServiceSubscriber serviceSubscriber)
        {
            if (command.LegalName.Edit)
            {
                serviceSubscriber.ChangeLegalName(command.LegalName.Value);
            }
        }

        private static void HandleDisplayNameChange(EditServiceSubscriber command, ServiceSubscriber serviceSubscriber)
        {
            if (command.DisplayName.Edit)
            {
                serviceSubscriber.ChangeDisplayName(command.DisplayName.Value);
            }
        }

        private static void HandleAddressChange(EditServiceSubscriber command, ServiceSubscriber serviceSubscriber)
        {
            if (command.Address.Edit)
            {
                serviceSubscriber.ChangeAddress(command.Address.Value);
            }
        }

        private static void HandleEmailChange(EditServiceSubscriber command, ServiceSubscriber serviceSubscriber)
        {
            if (command.Email.Edit)
            {
                serviceSubscriber.ChangeEmail(command.Email.Value);
            }
        }

        private static void HandlePhoneChange(EditServiceSubscriber command, ServiceSubscriber serviceSubscriber)
        {
            if (command.Phone.Edit)
            {
                serviceSubscriber.ChangePhone(command.Phone.Value);
            }
        }

        private static void HandleActiveStatusChange(EditServiceSubscriber command, ServiceSubscriber serviceSubscriber)
        {
            if (command.Active.Edit)
            {
                if (command.Active.Value)
                {
                    serviceSubscriber.Activate();
                }
                else
                {
                    serviceSubscriber.Deactivate();
                }
            }
        }
    }
}