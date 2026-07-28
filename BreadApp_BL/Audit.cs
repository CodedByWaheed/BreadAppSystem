using BreadApp_DL;
using BreadApp_Struct.AuthModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_BL
{
    public class Audit
    {
        public static List<AuditModel.AuditObjDTO> GetAuditData(AuditModel.AuditDTO dto)
        {
            return AuditData.GetAuditData(dto);
        }
    }
}
