using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_Struct.AuthModel
{
    public class AuthModel
    {
        public class AuthDTO
        {
            //public int? UserID { get; set; }
            public string? UsernameAttempted { get; set; } 
            public bool? Action {  get; set; }
           // public string? IpAddress { get; set; } 

        }
    }
}
