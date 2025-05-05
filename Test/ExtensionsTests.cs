using IdentityAuthModule.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Test
{
    public class ExtensionsTests
    {
        [Fact]
        public void AddAuthModule_ShouldThrow_WhenConnectionStringIsMissing()
        {
            // Arrange
            var settings = new Dictionary<string, string>
        {
            {"Jwt:Secret", "SuperSecretKey123"},
            {"Jwt:Issuer", "MyIssuer"},
            {"Jwt:Audience", "MyAudience"}
            // "ConnectionStrings:AuthConnection" is missing
        };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            var services = new ServiceCollection();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => services.AddAuthModule(config));
            Assert.Equal("Connection string 'AuthConnection' is missing.", ex.Message);
        }

        [Fact]
        public void AddAuthModule_ShouldThrow_WhenJwtSecretIsMissing()
        {
            // Arrange
            var settings = new Dictionary<string, string>
        {
            {"ConnectionStrings:AuthConnection", "FakeConnection"},
            {"Jwt:Issuer", "MyIssuer"},
            {"Jwt:Audience", "MyAudience"}
            // "Jwt:Secret" is missing
        };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            var services = new ServiceCollection();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => services.AddAuthModule(config));
            Assert.Equal("JWT configuration is missing or incomplete.", ex.Message);
        }

        [Fact]
        public void AddAuthModule_ShouldThrow_WhenJwtIssuerIsMissing()
        {
            // Arrange
            var settings = new Dictionary<string, string>
        {
            {"ConnectionStrings:AuthConnection", "FakeConnection"},
            {"Jwt:Secret", "SuperSecretKey123"},
            {"Jwt:Audience", "MyAudience"}
            // "Jwt:Issuer" is missing
        };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            var services = new ServiceCollection();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => services.AddAuthModule(config));
            Assert.Equal("JWT configuration is missing or incomplete.", ex.Message);
        }

        [Fact]
        public void AddAuthModule_ShouldNotThrow_WhenConfigIsComplete()
        {
            // Arrange
            var settings = new Dictionary<string, string>
        {
            {"ConnectionStrings:AuthConnection", "FakeConnection"},
            {"Jwt:Secret", "SuperSecretKey123"},
            {"Jwt:Issuer", "MyIssuer"},
            {"Jwt:Audience", "MyAudience"}
        };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            var services = new ServiceCollection();

            // Act
            var result = services.AddAuthModule(config);

            // Assert
            Assert.NotNull(result);
        }
    }
}
