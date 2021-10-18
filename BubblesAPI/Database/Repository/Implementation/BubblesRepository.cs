using System;
using System.Linq;
using System.Threading.Tasks;
using BubblesAPI.Database.Models;
using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using BubblesAPI.Helpers;

namespace BubblesAPI.Database.Repository.Implementation
{
    public class BubblesRepository : IBubblesRepository
    {
        private readonly BubblesContext _bubblesContext;

        public BubblesRepository(BubblesContext bubblesContext)
        {
            _bubblesContext = bubblesContext;
        }

        public async Task<string> SaveUser(RegisterUserRequest request)
        {
            User user;
            try{
                user = GetUserByEmail(request.Email);
            }
            catch (UserNotFoundException){
                user = null;
            }

            if (user == null){
                user = new User
                {
                    UserId = Utils.GenerateUserId(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    CreatedAt = DateTime.Now
                };
            }
            else{
                throw new UserAlreadyRegisterException();
            }

            var credentials = new Credentials
            {
                Email = user.Email,
                Password = request.Password,
                IsActive = true
            };
            _bubblesContext.Add(user);
            _bubblesContext.Add(credentials);
            await _bubblesContext.SaveChangesAsync();
            return user.UserId;
        }

        public User GetUserById(string userId)
        {
            try{
                return _bubblesContext.Users
                    .Single(x => x.UserId.Equals(userId));
            }
            catch (InvalidOperationException){
                throw new UserNotFoundException();
            }
        }

        public User GetUserByEmail(string emailId)
        {
            try{
                return _bubblesContext.Users
                    .Single(x => x.Email.Equals(emailId));
            }
            catch (InvalidOperationException){
                throw new UserNotFoundException();
            }
        }

        public User ValidateCredentials(LoginRequest request)
        {
            var credentials = _bubblesContext.Credentials.Single(x =>
                x.Email.Equals(request.Email) && x.Password.Equals(request.Password));
            if (credentials != null){
                return _bubblesContext.Users.Single(x => x.Email.Equals(request.Email));
            }

            throw new UserNotFoundException();
        }
    }
}