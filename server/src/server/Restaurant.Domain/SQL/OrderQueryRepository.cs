using Restaurant.Domain.Enumerations;

namespace Restaurant.Domain.SQL
{
    public class OrderQueryRepository
    {
        public static readonly string PendingOrdersByUserIdWithPagingQuery =
            $@"SELECT o.""Id"", ""CreatedOn"", ""Quantity"",
	          ""MealId"", m.""Name"" as ""MealName"", m.""Price"" as ""MealPrice""
	          FROM public.""Orders"" as o
	          INNER JOIN public.""Meals"" as m
	          ON o.""MealId"" = m.""Id""
              WHERE o.""OrderStatus"" = {(int)OrderStatus.Pending} 
                    AND o.""UserId"" = @UserID
	          ORDER BY ""CreatedOn"" DESC
	          LIMIT @LimitCount OFFSET @OffsetCount";

        public static readonly string PendingOrdersByUserIdAndMealIdWithPagingQuery =
            $@"SELECT o.""Id"", ""CreatedOn"", ""Quantity"",
	          ""MealId"", m.""Name"" as ""MealName"", m.""Price"" as ""MealPrice""
	          FROM public.""Orders"" as o
	          INNER JOIN public.""Meals"" as m
	          ON o.""MealId"" = m.""Id""
              WHERE o.""OrderStatus"" = {(int)OrderStatus.Pending} 
                    AND o.""UserId"" = '@UserID'
                    AND m.""Id"" = '@MealID'
	          ORDER BY ""CreatedOn"" DESC
	          LIMIT @LimitCount OFFSET @OffsetCount";
    }
}