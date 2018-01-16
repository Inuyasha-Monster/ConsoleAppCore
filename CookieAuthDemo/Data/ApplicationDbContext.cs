using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieAuthDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookieAuthDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
