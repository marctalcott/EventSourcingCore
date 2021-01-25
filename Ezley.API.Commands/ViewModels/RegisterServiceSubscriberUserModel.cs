using System;

namespace Ezley.API.Commands.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    namespace Ezley.API.Models
    {
        public class RegisterServiceSubscriberUserModel
        {
            [Required]
            public Guid UserId { get; set; }
            
            [Required]
            public Guid TenantId { get; set; }
            
            [Required]
            public Guid ServiceSubscriberId { get; set; }
            /// <summary>
            ///     An email address for user name.
            /// </summary>
            /// <value>The name of the user.</value>
            [Required]
            [MaxLength(256)]
            [EmailAddress]
            public string EmailAddress { get; set; }

            [Required]
            [MinLength(8)]
            [PasswordPropertyText]
            public string Password { get; set; }

        }
    }
}