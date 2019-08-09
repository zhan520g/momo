using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using momo.Application.Authorization.Secret;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace XUnit_momo
{
    //真实模式
    public class Hello
    {
        public HttpClient Client { get; }

        public ITestOutputHelper Output { get; }

        public Hello(ITestOutputHelper outputHelper)
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
            .UseStartup<Startup>());
            Client = server.CreateClient();
            Output = outputHelper;
        }
        [Fact]
        public async void Test1()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await Client.GetAsync($"/api/v1.0/Default/Get");

            // Output
            var responseText = await response.Content.ReadAsStringAsync();
            Output.WriteLine(responseText);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        }
    }
}
