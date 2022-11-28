using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace TaxManagement.Security
{
    public interface IUserRepository
    {
        Task<bool> Authenticate(string username, string password);
        Task<ActionResult<IEnumerable<string>>> GetUserNames();
    }
}
