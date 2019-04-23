namespace Restaurant.Domain.SQL
{
    public class UserAccountQueryRepository
    {
        public const string UserAccountsQuery = @"SELECT ""Id"", ""Email"",""FirstName"", ""LastName""
												 FROM public.""AspNetUsers""
												 ORDER BY ""FirstName"" ASC, ""LastName"" ASC";
    }
}
