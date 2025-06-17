using BoatBackend.Interfaces;
using BoatBackend.Models;
using BoatBackend.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace TestProject1;

public class AuthenticationServiceTest
{
    [Fact]
    public async Task Login_should_return_token()
    {
        // Arrange
        var userRepoMock = new Mock<IUserRepository>();
        const string testPw = "testPw";
        userRepoMock.Setup(u => u.GetUserByName(It.IsAny<string>())).ReturnsAsync(() => new User
        {
            Name = "test",
            Password = testPw
        });

        var sut = new AuthenticationService(userRepoMock.Object, NullLogger<AuthenticationService>.Instance);

        // Act
        var actual = await sut.Login(new User
        {
            Password = testPw
        });

        // Assert 
        Assert.NotEqual(string.Empty, actual);
    }
}