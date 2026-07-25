using BreadApp_Struct.AccessModel;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_DL
{
    public class AccessData
    {
        public static bool InsertAccess(SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertAccessLog", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var outputParam = new SqlParameter()
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    SetSessionContext(conn, sessionInfo);
                    cmd.ExecuteNonQuery();
                    return (int)outputParam.Value > 0;

                }
            }
        }


        //-----------------------------//////////////////////////------------------------------
        public static void SetSessionContext(SqlConnection conn, SessionContextInfo sessionInfo)
        {
            using var cmd = new SqlCommand(@"
                 EXEC sp_set_session_context @key = N'UserID', @value = @UserID;
                 EXEC sp_set_session_context @key = N'IPAddress', @value = @IPAddress;
                 EXEC sp_set_session_context @key = N'RequestID', @value = @RequestID;
                 EXEC sp_set_session_context @key = N'Action',    @value = @Action;
                 EXEC sp_set_session_context @key = N'Path',      @value = @Path;", conn);

            cmd.Parameters.AddWithValue("@UserID", (object?)sessionInfo.UserID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IPAddress", (object?)sessionInfo.IPAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RequestID", (object?)sessionInfo.RequestID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Action", (object?)sessionInfo.Action ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Path", (object?)sessionInfo.Path ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }
}
