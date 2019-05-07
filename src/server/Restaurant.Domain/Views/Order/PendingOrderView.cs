﻿using System;
using Restaurant.Domain.Views.Meal;

namespace Restaurant.Domain.Views.Order
{
    public class PendingOrderView
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public MealView Meal { get; set; }

        public int Quantity { get; set; }
    }
}
