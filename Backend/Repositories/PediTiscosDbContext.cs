using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Backend.Repositories;

public partial class PediTiscosDbContext : IdentityDbContext<ApplicationUser>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    public PediTiscosDbContext(DbContextOptions<PediTiscosDbContext> options) : base(options)
    {
    }
}
