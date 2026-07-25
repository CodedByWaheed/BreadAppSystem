namespace BreadApp_Struct.Common
{
    public class SessionContextInfo
    {
        public int? UserID { get; set; }
        public string? Role { get; set; }
        public string? IPAddress { get; set; }
        public Guid? RequestID { get; set;  }

    }
}
