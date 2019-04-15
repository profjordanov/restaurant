﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Persistence.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base (options)
        {
        }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<MealType> MealTypes { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureGuidPrimaryKeys();
            modelBuilder.ConfigureMealRestaurantRelations();
            modelBuilder.ConfigureMealAndTypeRelations();
            modelBuilder.ConfigureMealOrderRelations();
            modelBuilder.ConfigureUserOrdersRelations();
            modelBuilder.ConfigureRatingIndexes();
            modelBuilder.ConfigureUserRatingRelations();
            modelBuilder.ConfigureRestaurantRatingRelations();
            modelBuilder.ConfigureRestaurantTownRelations();
            modelBuilder.ConfigureRestaurantOwnerRelations();
            base.OnModelCreating(modelBuilder);
        }
    }
}
