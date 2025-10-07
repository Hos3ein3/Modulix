using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModuliX.BuildingBlocks.Context;
using ModuliX.BuildingBlocks.ValueGenerator;
using ModuliX.Identity.Domain.Models;

namespace ModuliX.Identity.Persistence.Context;

public class IdentityDbConfigurator : IModuleDbContextConfigurator
{
    public void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>().Property(x => x.GoogleId).HasMaxLength(250);
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.Property(e => e.Id).HasValueGenerator<GuidV7ValueGenerator>();
            entity.Property(e => e.GoogleId).HasMaxLength(250);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        entity.Property(e => e.Id)
            .HasValueGenerator<GuidV7ValueGenerator>()
        );

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            entity.Property(e => e.UserId)
                .HasValueGenerator<GuidV7ValueGenerator>()
        );
    }

    public void RegisterEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>();
        modelBuilder.Entity<AspNetRole>();
        modelBuilder.Entity<IdentityUserRole<Guid>>();
        modelBuilder.Entity<IdentityUserClaim<Guid>>();
        modelBuilder.Entity<IdentityRoleClaim<Guid>>();
        modelBuilder.Entity<IdentityUserLogin<Guid>>();
        modelBuilder.Entity<IdentityUserToken<Guid>>();
    }
}
