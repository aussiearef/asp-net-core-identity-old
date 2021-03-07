using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityNetCore.Models
{
    public class SigninViewModel
    {
        [Required(ErrorMessage = "User name must be provided.")]
        [DataType((DataType.EmailAddress))]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password must be provided.")]
        [DataType((DataType.Password))]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
