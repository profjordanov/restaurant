using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.SQL
{
    public class UserAccountQueryRepository
    {
        public const string UserAccountsQuery = @"SELECT ""Id"", ""Email"",""FirstName"", ""LastName""
												 FROM public.""AspNetUsers""";
    }
}
