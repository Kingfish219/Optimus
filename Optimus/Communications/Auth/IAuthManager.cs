using System.Threading.Tasks;

namespace Optimus.Communications.Auth
{
    public interface IAuthManager
    {
        Task<bool> SignIn(string username, string password);
    }
}
