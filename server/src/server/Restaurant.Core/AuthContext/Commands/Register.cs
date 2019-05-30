using Restaurant.Core._Base;

namespace Restaurant.Core.AuthContext.Commands
{
    public class Register : ICommand
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}