using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityNetCore.Models
{
    public class MFAViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Code { get; set; }

        public string QRCodeUrl { get; set; }
    }
}
