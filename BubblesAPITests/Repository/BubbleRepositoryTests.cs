using System.Threading.Tasks;
using BubblesAPI.Database;
using BubblesAPI.Database.Models;
using BubblesAPI.Database.Repository.Implementation;
using BubblesAPI.Exceptions;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BubblesAPITests.Repository
{
    public class BubbleRepositoryTests
    {
        private static DbContextOptions<BubblesContext> GetDbOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<BubblesContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        [Fact]
        public async Task ShouldReturnUserDataWithValidUserId()
        {
            var options = GetDbOptions("ShouldReturnUserDataWithValidIdDb");
            const string someUserId = "user-id";
            await using var context = new BubblesContext(options);
            var repository = new BubblesRepository(context);
            var userData = Builder<User>.CreateNew()
                .With(x => x.UserId = someUserId)
                .Build();
            context.Add(userData);
            await context.SaveChangesAsync();

            var result = repository.GetUserById(someUserId);
            Assert.Equal(someUserId, result.UserId);
            ;
        }

        [Fact]
        public void ShouldReturnNullWhenInvalidUserIdIsPassed()
        {
            var options = GetDbOptions("ShouldReturnUserDataWithInValidIdDb");
            const string someUserId = "some-user-id";
            var context = new BubblesContext(options);
            var repository = new BubblesRepository(context);

            Assert.Throws<UserNotFoundException>(() => repository.GetUserById(someUserId));
        }

        [Fact]
        public async Task ShouldReturnUserDataWhenValidEmailIsPassed()
        {
            const string someEmailId = "someEmailId@example.com";
            var options = GetDbOptions("UserWithValidEmailDb");
            var context = new BubblesContext(options);
            var repository = new BubblesRepository(context);
            var user = Builder<User>
                .CreateNew()
                .With(x => x.Email = someEmailId).Build();
            context.Add(user);
            await context.SaveChangesAsync();

            var result = repository.GetUserByEmail(someEmailId);
            Assert.Equal(someEmailId, result.Email);
        }
    }
}