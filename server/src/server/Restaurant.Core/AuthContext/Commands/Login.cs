using Restaurant.Core._Base;
using Restaurant.Domain.Views.Auth;

namespace Restaurant.Core.AuthContext.Commands
{
    public class Login : ICommand<JwtView>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}