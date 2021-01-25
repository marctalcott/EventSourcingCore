using Ezley.ValueObjects;

namespace Ezley.API.Commands.ViewModels
{
    public class EditPromiseeViewModel
    {
        public Editable<string> LegalName { get; set; }
        public Editable<string> DisplayName { get; set; }
        public Editable<Address> Address { get; set; }
        public Editable<Phone> Phone { get; set; }
        public Editable<Email> Email { get; set; }
    }

}