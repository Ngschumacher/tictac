using System.Collections.Generic;
 using TicTac.Core.Models;
 
 namespace TicTac.Core.Interfaces {
     public interface IUserService {
         User GetOrCreateUser(string username);
         List<User> GetUsers(List<int> userIds);
     }
 }