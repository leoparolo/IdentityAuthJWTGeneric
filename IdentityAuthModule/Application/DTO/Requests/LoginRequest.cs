namespace IdentityAuthModule.Application.DTO.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
