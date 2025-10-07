
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModuliX.Identity.Domain.Models;
using ZstdSharp.Unsafe;

namespace ModuliX.Identity.Persistence.Context;

public class ApplicationIdentityDbContext : IdentityDbContext<AspNetUser, AspNetRole, Guid>
{

    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
