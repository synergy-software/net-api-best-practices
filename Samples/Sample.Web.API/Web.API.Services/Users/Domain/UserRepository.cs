using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;

namespace Synergy.Samples.Web.API.Services.Users.Domain
{
    [CreatedImplicitly]
    public class UserRepository : IUserRepository
    {
        private readonly List<User> users = new List<User>();

        public Task<User> CreateUser(string login)
        {
            var user = new User(Guid.NewGuid().ToString().Replace("-", ""), login);
            users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User?> FindUserBy(string userId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            return Task.FromResult(user);
        }

        private async Task<User> GetUserBy(string userId)
        {
            var user = await FindUserBy(userId);
            Fail.IfNull(user, Violation.Of("There is no user with id '{0}'", userId));
            return user;
        }

        public async Task DeleteUser(string userId)
        {
            var user = await GetUserBy(userId);
            users.Remove(user);
        }

        public Task<ReadOnlyCollection<User>> GetAllUsers()
        {
            return Task.FromResult(users.AsReadOnly());
        }
    }

    public interface IUserRepository
    {
        Task<User> CreateUser(string login);
        Task<User?> FindUserBy(string userId);
        Task DeleteUser(string userId);
        Task<ReadOnlyCollection<User>> GetAllUsers();
    }
}