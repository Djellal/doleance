using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using Doleance.Models.AppDb;
using Doleance.Models;

namespace Doleance.Data
{
    public partial class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        partial void OnModelBuilding(ModelBuilder builder)
        {
            //builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        }
    }
}
