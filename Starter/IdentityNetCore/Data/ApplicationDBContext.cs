using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityNetCore.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext()
        { }

        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
