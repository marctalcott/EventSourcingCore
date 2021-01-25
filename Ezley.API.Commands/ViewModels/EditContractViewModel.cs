using System;
using Ezley.Domain.ContractManagement;
using Ezley.ValueObjects;

namespace Ezley.API.Commands.ViewModels
{
    public class EditContractViewModel
    {
        //internal Guid Id { get; set; }
        public Editable<Guid> PromisorId { get; set; }
        public Editable<DateTime> StartDate { get; set; }
        public Editable<DateTime> EndDate { get; set; }
        public Editable<string> Name { get; set; }
        public Editable<string> Description { get; set; }
        public Editable<ContractStage> ContractStage { get; set; }
        public Editable<ContractStatus> ContractStatus { get; set; }
        public Editable<SignatureStatus> SignatureStatus { get; set; }
    }

}