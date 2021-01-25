using System;
using System.ComponentModel.DataAnnotations;
using Ezley.ValueObjects;

namespace Ezley.API.Commands.ViewModels
{
    public class RegisterTenantViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public string LegalName { get; set; }
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }
        public Email Email { get; set; }

    }
 
}