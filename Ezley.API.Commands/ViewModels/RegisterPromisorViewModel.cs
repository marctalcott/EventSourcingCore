using System;
using System.ComponentModel.DataAnnotations;
using Ezley.ValueObjects;

namespace Ezley.API.Commands.ViewModels
{
    public class RegisterPromisorViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid TenantId { get; set; }
        
        public string LegalName { get; set; }
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }
        public Email Email { get; set; }

    }
 
}