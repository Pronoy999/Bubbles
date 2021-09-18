using System.Threading.Tasks;
using BubblesAPI.Database.Models;
using BubblesAPI.DTOs;
using BubblesAPI.Helpers;

namespace BubblesAPI.Database.Repository.Implementation
{
    public class BubblesRepository : IBubblesRepository
    {
        private BubblesContext _bubblesContext;

        public BubblesRepository(BubblesContext bubblesContext)
        {
            _bubblesContext = bubblesContext;
        }

        public async Task<string> SaveUser(RegisterUserRequest request)
        {
            /*var user = await GetUserById(request.UserId);
            if (user == null)
            {
                user = new User
                {
                    UserId = Utils.GenerateUserId(),
                    FirstName = request.FirstName,
                    LastName = request.LastName, 
                    Email = request.Email,
                    UserStatus = new Status{Id = Constants.StatusInitialises}
                };
            }else{
                
            }*/throw new System.NotImplementedException();
        }

        public Task<User> GetUserById(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetUserByEmail(string emailId)
        {
            throw new System.NotImplementedException();
        }
    }
}