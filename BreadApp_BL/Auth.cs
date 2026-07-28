using BreadApp_DL;
using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_BL
{
    public class Auth
    {
        public static bool InsertAuthAction(AuthModel.AuthDTO auth, SessionContextInfo sessionInfo)
        {
            return AuthData.InsertAuthAction(auth, sessionInfo);
        }
        public static List<AuthModel.AuthObjDTO> GetAuthData(AuthModel.AuthUserDTO dto)
        {
            return AuthData.GetAuthData(dto);
        }
    }
}
