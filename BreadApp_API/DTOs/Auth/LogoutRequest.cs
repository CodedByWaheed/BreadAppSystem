namespace BreadApp_API.DTOs.Auth
{
    public class LogoutRequest
    {
        public string NationalNum { get; set; }
        public string RefreshToken { get; set; }
    }
}
