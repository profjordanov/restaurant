using System;

namespace Restaurant.Domain.Views.Meal
{
    public class MealView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
