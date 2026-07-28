using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_Struct.AuthModel
{
    public class AccessModel
    {
        
        public class AccessDTO
        {
            public int? UserID { get; set; } = null;
            public string? HttpMethod { get; set; } = null;
            public string? EndPoint { get; set; } = null;
            public string? IPAddress { get; set; } = null;
            public Guid? RequestedID { get; set; } = null;
            public DateTime? DateFrom { get; set; } = DateTime.Today;
            public DateTime? DateTo { get; set; } = DateTime.Today.AddDays(1);
            public int? PageNumber { get; set; } = 1;
            public int? PageRow { get; set; } = 20;

        }
        public class AccessObjDTO
        {
            public int? ID { get; set; }
            public int? UserID { get; set; }
            public string? IPAddress { get; set; }
            public string? Path { get; set; }
            public string? Action { get; set; }
            public Guid? RequestedID { get; set; }
            public DateTime? TimeStamp { get; set; }
        }
    }
}
