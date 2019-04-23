using Restaurant.Core._Base;
using Restaurant.Domain.Views.Auth;
using System.Collections.Generic;

namespace Restaurant.Core.AuthContext.Queries
{
    public class GetAllUserAccounts : IQuery<IList<UserView>>
    {
    }
}
