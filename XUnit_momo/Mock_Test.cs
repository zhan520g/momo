using momo.Application.Authorization.Secret;
using momo.Application.Authorization.Secret.Dto;
using momo.Domain.Authorization.Secret;
using momo.Entity.Premission;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace XUnit_momo
{
    public class Mock_Test
    {
        ITestOutputHelper testOutputHelper;
        public Mock_Test(ITestOutputHelper outputHelper)
        {
            testOutputHelper = outputHelper;
        }

        [Fact]
        public void Test() => testOutputHelper.WriteLine("this is outputHelper");

        [Fact]
        public void GetUser()
        {
            // Arrange
            var mockObject = new Mock<ISecretDomain>();
            IdentityUser user = new IdentityUser();
            user.Name = "zs";
            user.Id = new System.Guid();
            user.Password = "222";
            user.Account = "222";
            string account = "zs";
            string password = "123456";
            mockObject.Setup(domian => domian.GetUserForLoginAsync(account, password)).Returns(System.Threading.Tasks.Task.FromResult(user));

            // Act
            var userApplication = new SecretAppService(mockObject.Object);
            var userName = userApplication.GetCurrentUserAsync(account, password).Result;

            //// Assert
            Assert.IsType<UserDto>(userName);
        }
    }
}
