using IdentityAuthModule.CredentialValidator;
using IdentityAuthModule.DTO;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Test
{
    public class CredentialValidatorTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly ICredentialValidator _validator;

        public CredentialValidatorTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            _validator = new CredentialValidator(_userManagerMock.Object);
        }

        [Fact]
        public async Task ValidateAsync_WithValidCredentials_ReturnsUser()
        {
            LoginRequest validar = new LoginRequest
            {
                UserName = "leo",
                Password = "1234"
            };

            var user = new IdentityUser { UserName = validar.UserName};

            _userManagerMock.Setup(x => x.FindByNameAsync(validar.UserName)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, validar.Password)).ReturnsAsync(true);
            

            var result = await _validator.ValidateAsync(validar);

            Assert.Equal("leo", result.UserName);
        }

        [Fact]
        public async Task ValidateAsync_WithEmptyCredentials_ThrowsUnauthorized()
        {
            LoginRequest validar = new LoginRequest
            {
                UserName = "",
                Password = " "
            };

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _validator.ValidateAsync(validar));

            Assert.Equal("Ingrese su credencial.", ex.Message);
        }

        [Fact]
        public async Task ValidateAsync_WithWrongPassword_ThrowsUnauthorized()
        {
            LoginRequest validar = new LoginRequest
            {
                UserName = "leo",
                Password = "wrong"
            };

            var user = new IdentityUser { UserName = validar.UserName };

            _userManagerMock.Setup(x => x.FindByNameAsync(validar.UserName)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, validar.Password)).ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _validator.ValidateAsync(validar));

            Assert.Equal("Credenciales inválidas.", ex.Message);
        }
    }
}
