using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Pkcs;
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
        public static List<AccessModel.AccessObjDTO> GetAccessData(AccessModel.AccessDTO dto)
        {
            List<AccessModel.AccessObjDTO> List = new List<AccessModel.AccessObjDTO>();
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAccessLogs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", (object?)dto.UserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@HttpMethod", (object?)dto.HttpMethod ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndPoint", (object?)dto.EndPoint ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IpAddress", (object?)dto.IPAddress ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestedID", (object?)dto.RequestedID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateFrom", (object?)dto.DateFrom ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateTo", (object?)dto.DateTo ?? DBNull.Value);
                   

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
        public static AccessModel.AccessObjDTO? GetAccessDataBy(AccessModel.AccessDTO dto)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAccessLogs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", (object?)dto.UserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@HttpMethod", (object?)dto.HttpMethod ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndPoint", (object?)dto.EndPoint ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IpAddress", (object?)dto.IPAddress ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestedID", (object?)dto.RequestedID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateFrom", (object?)dto.DateFrom ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateTo", (object?)dto.DateTo ?? DBNull.Value);


                    var outputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return MapToObj(reader);
                    }
                }
            }
            return null;
        }
        //-----------------------------/////////////MAPPER/////////////------------------------------
        private static AccessModel.AccessObjDTO MapToObj(SqlDataReader reader)
        {
            return new AccessModel.AccessObjDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID"))? null:reader.GetInt32(reader.GetOrdinal("ID")),
                UserID = reader.IsDBNull(reader.GetOrdinal("UserID"))? null:reader.GetInt32(reader.GetOrdinal("UserID")),
                IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress"))? null:reader.GetString(reader.GetOrdinal("IPAddress")),
                Path = reader.IsDBNull(reader.GetOrdinal("Path"))? null:reader.GetString(reader.GetOrdinal("Path")),
                Action = reader.IsDBNull(reader.GetOrdinal("Action"))? null:reader.GetString(reader.GetOrdinal("Action")),
                RequestedID = reader.IsDBNull(reader.GetOrdinal("RequestedID"))? null:reader.GetGuid(reader.GetOrdinal("RequestedID")),
                TimeStamp = reader.IsDBNull(reader.GetOrdinal("TimeStamp"))? null:reader.GetDateTime(reader.GetOrdinal("TimeStamp"))
            };
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
