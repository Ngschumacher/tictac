using System.Collections.Generic;
using TypeFaster.Models;

namespace TypeFaster.Core.Interfaces
{
    public interface IUserService
    {
        User GetOrCreateUser(string username);
        User GetUser(int userId);
        List<User> GetUsers(List<int> userIds);
    }
}