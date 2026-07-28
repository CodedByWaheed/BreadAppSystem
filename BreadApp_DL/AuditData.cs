using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BreadApp_DL
{
    public class AuditData
    {
      
        public static List<AuditModel.AuditObjDTO> GetAuditData(AuditModel.AuditDTO dto)
        {
            List<AuditModel.AuditObjDTO> List = new List<AuditModel.AuditObjDTO>();
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAuditLogs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", (object?)dto.UserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Action", (object?)dto.Action ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TableName", (object?)dto.TableName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RecordID", (object?)dto.RecordID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IPAddress", (object?)dto.IpAddress ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Role", (object?)dto.Role ?? DBNull.Value);
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

        //-----------------------------/////////////MAPPER/////////////------------------------------
        private static AuditModel.AuditObjDTO MapToObj(SqlDataReader reader)
        {
            return new AuditModel.AuditObjDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID"))?null:reader.GetInt32(reader.GetOrdinal("ID")),
                UserID = reader.IsDBNull(reader.GetOrdinal("UserID"))?null:reader.GetInt32(reader.GetOrdinal("UserID")),
                Role =reader.IsDBNull(reader.GetOrdinal("Role"))?null: reader.GetString(reader.GetOrdinal("Role")), 
                IpAddress =reader.IsDBNull(reader.GetOrdinal("IPAddress"))?null: reader.GetString(reader.GetOrdinal("IPAddress")),
                RequestedID = reader.IsDBNull(reader.GetOrdinal("RequestID"))?null:reader.GetGuid(reader.GetOrdinal("RequestID")),
                Action = reader.IsDBNull(reader.GetOrdinal("Action"))?null: reader.GetString(reader.GetOrdinal("Action")),
                TableName = reader.IsDBNull(reader.GetOrdinal("TableName"))?null:reader.GetString(reader.GetOrdinal("TableName")),
                RecordID = reader.IsDBNull(reader.GetOrdinal("RecordID"))?null:reader.GetInt32(reader.GetOrdinal("RecordID")),
                TimeSpan = reader.IsDBNull(reader.GetOrdinal("Time"))?null:reader.GetDateTime(reader.GetOrdinal("Time"))
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
