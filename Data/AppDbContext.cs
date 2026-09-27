using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using Doleance.Models.AppDb;

namespace Doleance.Data
{
  public partial class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }

    public AppDbContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Doleance.Models.AppDb.Qualite>().HasNoKey();
        builder.Entity<Doleance.Models.AppDb.Rendezvou>()
              .HasOne(i => i.Structure)
              .WithMany(i => i.Rendezvous)
              .HasForeignKey(i => i.structId)
              .HasPrincipalKey(i => i.Id);

        builder.Entity<Doleance.Models.AppDb.Doleanctab>()
              .Property(p => p.userid)
              .HasDefaultValueSql("\"-\"");

        builder.Entity<Doleance.Models.AppDb.Doleanctab>()
              .Property(p => p.repondu)
              .HasDefaultValueSql("0");

        builder.Entity<Doleance.Models.AppDb.Rendezvou>()
              .Property(p => p.num)
              .HasDefaultValueSql("\"\"");

        builder.Entity<Doleance.Models.AppDb.Rendezvou>()
              .Property(p => p.objet)
              .HasDefaultValueSql("\"\"");

        builder.Entity<Doleance.Models.AppDb.Rendezvou>()
              .Property(p => p.nomPrenom)
              .HasDefaultValueSql("\"\"");

        builder.Entity<Doleance.Models.AppDb.Rendezvou>()
              .Property(p => p.acceptee)
              .HasDefaultValueSql("false");

        builder.Entity<Doleance.Models.AppDb.Rendezvou>()
              .Property(p => p.textReponse)
              .HasDefaultValueSql("\"\"");

        builder.Entity<Doleance.Models.AppDb.Structure>()
              .Property(p => p.Designation)
              .HasDefaultValueSql("\"\"");

        this.OnModelBuilding(builder);
    }


    public DbSet<Doleance.Models.AppDb.Appartenance> Appartenances
    {
      get;
      set;
    }

    public DbSet<Doleance.Models.AppDb.Doleanctab> Doleanctabs
    {
      get;
      set;
    }

    public DbSet<Doleance.Models.AppDb.Qualite> Qualites
    {
      get;
      set;
    }

    public DbSet<Doleance.Models.AppDb.Rendezvou> Rendezvous
    {
      get;
      set;
    }

    public DbSet<Doleance.Models.AppDb.Structure> Structures
    {
      get;
      set;
    }

  }
}
