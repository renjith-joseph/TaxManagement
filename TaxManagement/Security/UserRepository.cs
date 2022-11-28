using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace TaxManagement.Security
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>
        {
            new User
            {
                Id = 1, Username = "danskeuser", Password = "dansbe@123"
            }
        };
        public async Task<bool> Authenticate(string username, string password)
        {
            if (await Task.FromResult(_users.SingleOrDefault(x => x.Username == username && x.Password == password)) != null)
            {
                return true;
            }
            return false;
        }
        public async Task<ActionResult<IEnumerable<string>>> GetUserNames()
        {
            List<string> users = new List<string>();
            foreach (var user in _users)
            {
                users.Add(user.Username);
            }
            return await Task.FromResult(users);
        }

        //Task<NUnit.Framework.List> IUserRepository.GetUserNames()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
