﻿using Koi.Repositories.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories
{
    public class KoiFarmShopDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<KoiFish> KoiFishs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<KoiBreed> KoiBreeds { get; set; }
        public DbSet<KoiFishKoiBreed> KoiFishKoiBreeds { get; set; }

        public KoiFarmShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship
            modelBuilder.Entity<KoiFishKoiBreed>()
                .HasKey(kfkb => new { kfkb.KoiFishId, kfkb.KoiBreedId });

            modelBuilder.Entity<KoiFishKoiBreed>()
                .HasOne(kfkb => kfkb.KoiFish)
                .WithMany(kf => kf.KoiFishKoiBreeds)
                .HasForeignKey(kfkb => kfkb.KoiFishId);

            modelBuilder.Entity<KoiFishKoiBreed>()
                .HasOne(kfkb => kfkb.KoiBreed)
                .WithMany(kb => kb.KoiFishKoiBreeds)
                .HasForeignKey(kfkb => kfkb.KoiBreedId);

            modelBuilder.Entity<OrderDetail>()
        .HasOne(od => od.Order)
        .WithMany(o => o.OrderDetails)
        .HasForeignKey(od => od.OrderId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}