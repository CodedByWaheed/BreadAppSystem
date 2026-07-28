using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BreadApp_Struct.AuthModel
{
    public class AuthModel
    {
        public class AuthDTO
        {
           
            public string? UsernameAttempted { get; set; } 
            public bool? Action {  get; set; }
         
        }

        public class AuthUserDTO
        {
            public int? UserID { get; set; } = null;
            public bool? Success { get; set; } = null;
            public string? IPAddress { get; set; } = null;
            public Guid? RequestedId { get; set; } = null;
            public DateTime? DateFrom { get; set; } = DateTime.Today;
            public DateTime? DateTo { get; set; } = DateTime.Today.AddDays(1);
            public int? PageNumber { get; set; } = 1;
            public int? PageRow { get; set; } = 20;

        }
        public class AuthObjDTO
        {
            public int? ID { get; set; } = null;
            public int? UserID { get; set; } = null;
            public string? UsernameAttempted { get; set; } = null;
            public bool? Success { get; set; } = null;
            public string? IpAddress { get; set; } = null;
            public Guid? RequestedID { get; set; } = null;
            public DateTime? TimeSpan { get; set; } = null; 
        }
    }
}
