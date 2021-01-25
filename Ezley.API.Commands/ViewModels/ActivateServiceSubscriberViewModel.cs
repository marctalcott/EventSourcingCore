using System;
using System.ComponentModel.DataAnnotations;


namespace Ezley.API.Commands.ViewModels
{
    public class ActivateServiceSubscriberViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }

}