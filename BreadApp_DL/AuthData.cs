using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BreadApp_DL.UserModel;

namespace BreadApp_DL
{
    public class AuthData
    {
       
        public static bool InsertAuthAction(AuthModel.AuthDTO auth , SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertAuthenticationLog", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UsernameAttempted", (object?)auth.UsernameAttempted ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Action", (object?)auth.Action ?? DBNull.Value);
                    var outputParam = new SqlParameter()
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(outputParam);
                    conn.Open();
                    SetSessionContext(conn, sessionInfo);

                    cmd.ExecuteNonQuery();
                    return (int)outputParam.Value> 0;
                }
            }

        }

        //-----------------------------//////////////////////////------------------------------
        public static void SetSessionContext(SqlConnection conn, SessionContextInfo sessionInfo)
        {
            //EXEC sp_set_session_context @key = N'Role', @value = @Role;
            //
            using var cmd = new SqlCommand(@"
                 EXEC sp_set_session_context @key = N'UserID', @value = @UserID;
                 EXEC sp_set_session_context @key = N'RequestID', @value = @RequestID;
                 EXEC sp_set_session_context @key = N'IPAddress', @value = @IPAddress;
                 ", conn);

            cmd.Parameters.AddWithValue("@UserID", (object?)sessionInfo.UserID ?? DBNull.Value);
           // cmd.Parameters.AddWithValue("@Role", (object?)sessionInfo.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IPAddress", (object?)sessionInfo.IPAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RequestID", (object?)sessionInfo.RequestID ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

    }
}
