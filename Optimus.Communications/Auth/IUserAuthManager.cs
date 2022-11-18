using System.Threading.Tasks;

namespace Optimus.Communications.Auth
{
    public interface IUserAuthManager
    {
        Task<bool> SignIn(string username, string password);
    }
}
