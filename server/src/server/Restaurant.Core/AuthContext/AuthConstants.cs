using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Core.AuthContext
{
    public static class AuthConstants
    {
        public static class ClaimTypes
        {
            public const string IsAdmin = "isAdmin";
        }

        public static class Policies
        {
            public const string IsAdmin = "IsAdmin";
        }
    }
}
