namespace Restaurant.Core.MealContext.HttpRequests
{
    public class RegisterMealRequest
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string TypeId { get; set; }

        public string RestaurantId { get; set; }
    }
}