using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;

namespace BreadApp_BL
{
    public class Auth
    {
        public static bool InsertAuthAction(AuthModel.AuthDTO auth, SessionContextInfo sessionInfo)
        {
            return BreadApp_DL.AuthData.InsertAuthAction(auth, sessionInfo);
        }
    }
}
