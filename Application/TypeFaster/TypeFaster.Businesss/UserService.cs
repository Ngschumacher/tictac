using System.Collections.Generic;
using System.Linq;
using TypeFaster.Core.Interfaces;
using TypeFaster.DataAccess.Contexts;
using TypeFaster.Models;

namespace TypeFaster.Business
{
    public class UserService : IUserService
    {
        public User GetOrCreateUser(string username)
        {
            using (var context = new GameContext())
            {
                var user = context.User.FirstOrDefault(x => x.Username == username);

                if (user == null)
                {
                    user = new User()
                    {
                        Username = username
                    };
                    context.User.Add(user);
                }

                context.SaveChanges();

                return user;

            }
        }

        public User GetUser(int userId)
        {
            using (var context = new GameContext())
            {
                return context.User.FirstOrDefault(x => x.Id == userId);
            }
        }
        
        public List<User> GetUsers(List<int> userIds)
        {
            using (var context = new GameContext())
            {
                return context.User.Where(x => userIds.Contains(x.Id)).ToList();
            }
        }
    }
}