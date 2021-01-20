using Roster.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Roster.Core.Service.Interfaces
{
    public interface IAuthService
    {
        Task<TokenModel> LogUserIn(LoginModel model);
        Task<(UserModel user, string message)> RegisterUser(SignUpModel model);
    }
}
