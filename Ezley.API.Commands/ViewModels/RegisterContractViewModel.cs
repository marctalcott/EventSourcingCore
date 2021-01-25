using System;
using System.ComponentModel.DataAnnotations;

namespace Ezley.API.Commands.ViewModels
{
    public class RegisterContractViewModel
    {
        [Required]
        public Guid Id { get;  }
        [Required]
        public Guid TenantId { get;  }
        [Required]
        public Guid PromiseeId { get;  }
        public Guid PromisorId { get;  }
        public string Name { get;  }
        public string Description { get;  }

    }
 
}