using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadApp_DL;
using BreadApp_Struct.AccessModel;
using BreadApp_Struct.Common;

namespace BreadApp_BL
{
    public class Access
    {
        public static bool Insert(SessionContextInfo sessionInfo)
        {
            return AccessData.InsertAccess(sessionInfo);
        }
    }
}
