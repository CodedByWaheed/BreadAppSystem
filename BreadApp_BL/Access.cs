using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadApp_DL;
using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;

namespace BreadApp_BL
{
    public class Access
    {
        public static bool Insert(SessionContextInfo sessionInfo)
        {
            return AccessData.InsertAccess(sessionInfo);
        }
        public static List<AccessModel.AccessObjDTO> GetAccessData(AccessModel.AccessDTO dto )
        {
            return AccessData.GetAccessData(dto);
        }
        public static AccessModel.AccessObjDTO? GetAccessDataBy(AccessModel.AccessDTO dto)
        {
            return AccessData.GetAccessDataBy(dto);
        }
    }
}
