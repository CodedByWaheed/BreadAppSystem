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

        public static List<AuthModel.AuthObjDTO> GetAuthData(AuthModel.AuthUserDTO dto)
        {
            List<AuthModel.AuthObjDTO> List = new List<AuthModel.AuthObjDTO>();
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAuthLogs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", (object?)dto.UserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Success", (object?)dto.Success ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IPAdress", (object?)dto.IPAddress ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestedID", (object?)dto.RequestedId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateFrom", (object?)dto.DateFrom ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateTo", (object?)dto.DateTo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNumber", (object?)dto.PageNumber ?? 1);
                    cmd.Parameters.AddWithValue("@PageRow", (object?)dto.PageRow ?? 20);
                    var outputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List.Add(MapToObj(reader));
                    }
                }
            }
            return List;
        }

        //-----------------------------/////////////MAPPER/////////////------------------------------
        private static AuthModel.AuthObjDTO MapToObj(SqlDataReader reader)
        {
            return new AuthModel.AuthObjDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID"))?null:reader.GetInt32(reader.GetOrdinal("ID")),
                UserID = reader.IsDBNull(reader.GetOrdinal("UserID"))?null :reader.GetInt32(reader.GetOrdinal("UserID")),
                UsernameAttempted = reader.IsDBNull(reader.GetOrdinal("UsernameAttempted"))?null:reader.GetString(reader.GetOrdinal("UsernameAttempted")),
                Success = reader.IsDBNull(reader.GetOrdinal("Action"))? null:reader.GetBoolean(reader.GetOrdinal("Action")),
                IpAddress =reader.IsDBNull(reader.GetOrdinal("IPAddress"))?null: reader.GetString(reader.GetOrdinal("IPAddress")),
                RequestedID =reader.IsDBNull(reader.GetOrdinal("RequestedID"))?null: reader.GetGuid(reader.GetOrdinal("RequestedID")),
                TimeSpan = reader.IsDBNull(reader.GetOrdinal("TImeSpan"))?null:reader.GetDateTime(reader.GetOrdinal("TimeSpan"))
            };
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
