namespace Restaurant.Domain.SQL
{
    public class RestaurantQueryRepository
    {
        public const string RestaurantsByTownQuery =
            @"SELECT res.""Id"", ""Name"", ROUND(AVG(rat.""Stars""),2) as ""AverageRating"", ""TownId""
			  FROM public.""Restaurants"" as res
			  INNER JOIN public.""Ratings"" as rat
			  ON rat.""RestaurantId"" = res.""Id""
			  WHERE ""TownId"" = @TownID   
			  GROUP BY res.""Id"", ""Name"", ""TownId""";
    }
}
