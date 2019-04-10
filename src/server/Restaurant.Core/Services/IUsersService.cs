using Optional;
using Restaurant.Core.Models;
using System.Threading.Tasks;

namespace Restaurant.Core.Services
{
    public interface IUsersService
    {
        Task<Option<JwtModel, Error>> Login(LoginUserModel model);

        Task<Option<UserModel, Error>> Register(RegisterUserModel model);
    }
}
