using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Persistence.EntityFramework
{
    internal static class OnModelCreatingConfiguration
    {
        internal static void ConfigureGuidPrimaryKeys(this ModelBuilder builder)
        {
            builder
                .Entity<Domain.Entities.Restaurant>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Entity<Meal>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Entity<MealType>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Entity<Order>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Entity<Rating>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Entity<Town>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }

        internal static void ConfigureMealRestaurantRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Meal>()
                .HasOne(meal => meal.Restaurant)
                .WithMany(restaurant => restaurant.Meals)
                .HasForeignKey(meal => meal.RestaurantId);
        }

        internal static void ConfigureMealAndTypeRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Meal>()
                .HasOne(meal => meal.Type)
                .WithMany(type => type.Meals)
                .HasForeignKey(meal => meal.TypeId);
        }

        internal static void ConfigureMealOrderRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Order>()
                .HasOne(order => order.Meal)
                .WithMany(meal => meal.Orders)
                .HasForeignKey(order => order.MealId);
        }

        internal static void ConfigureUserOrdersRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Order>()
                .HasOne(order => order.User)
                .WithMany(user => user.MadeOrders)
                .HasForeignKey(order => order.UserId);
        }

        internal static void ConfigureRatingIndexes(this ModelBuilder builder)
        {
            builder
                .Entity<Rating>()
                .HasIndex(rating => new
                {
                    rating.RestaurantId,
                    rating.UserId
                })
                .IsUnique();
        }

        internal static void ConfigureUserRatingRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Rating>()
                .HasOne(rating => rating.User)
                .WithMany(user => user.GivenRatings)
                .HasForeignKey(rating => rating.UserId);
        }

        internal static void ConfigureRestaurantRatingRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Rating>()
                .HasOne(rating => rating.Restaurant)
                .WithMany(user => user.Ratings)
                .HasForeignKey(rating => rating.RestaurantId);
        }

        internal static void ConfigureRestaurantTownRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Domain.Entities.Restaurant>()
                .HasOne(restaurant => restaurant.Town)
                .WithMany(town => town.Restaurants)
                .HasForeignKey(restaurant => restaurant.TownId);
        }

        internal static void ConfigureRestaurantOwnerRelations(this ModelBuilder builder)
        {
            builder
                .Entity<Domain.Entities.Restaurant>()
                .HasOne(restaurant => restaurant.Owner)
                .WithMany(user => user.OwnedRestaurants)
                .HasForeignKey(restaurant => restaurant.OwnerId);
        }
    }
}