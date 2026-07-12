namespace BreadApp_API.DTOs.Auth
{
    public class RefreshRequest
    {
        public string RefreshToken { get; set; }
        public string NationalNum { get; set; }
    }
}
