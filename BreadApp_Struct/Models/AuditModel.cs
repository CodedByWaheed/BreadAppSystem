using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_Struct.AuthModel
{
    public class AuditModel
    {

        public class AuditDTO
        {
            public int? UserID { get; set; } = null;
            public string?Action { get; set; }
            public string? TableName { get; set; } = null;
            public int? RecordID { get; set; } = null;
            public string? IpAddress { get; set; } = null;
            public string?Role { get; set; } = null;
            public DateTime? DateFrom { get; set; } = DateTime.Today;
            public DateTime? DateTo { get; set; } = DateTime.Today.AddDays(1);
            public int? PageNumber { get; set; } = 1;
            public int? PageRow { get; set; } = 20;
        


        }

        public class AuditObjDTO
        {
            public int? ID { get; set; } = null;
            public int? UserID { get; set; } = null;
            public string? Role { get; set; } = null;
            public string? IpAddress { get; set; } = null;
            public Guid? RequestedID { get; set; } = null;
            public string? Action { get; set; } = null;
            public string? TableName { get; set; } = null;
            public int? RecordID { get; set; } = null;
            public DateTime? TimeSpan { get; set; } = null;

        }
    }
}
