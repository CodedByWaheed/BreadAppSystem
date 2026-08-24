
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BreadApp_DL
{
    public class QRModel
    {
        /// <summary>
        /// Data coming FROM the front end (create-a-QR request).
        /// </summary>
        public class QrDataDTO
        {
            public int UserID { get; set; }
            public int BreadPointID { get; set; }
            public int PortionCount { get; set; }
           // public decimal PortionPrice { get; set; }
        }

        /// <summary>
        /// Data we give back TO the front end.
        /// </summary>
        public class QrInfoDTO
        {
            public QrInfoDTO(int QRCodeID, Guid PublicID, int UserID, int BreadPointID,
                int PortionCount, int Status, byte[] Token,
                DateTime CreatedAt, DateTime ExpiresAt, DateTime? ScannedAt, bool IsScanned)
            {
                this.QRCodeID = QRCodeID;
                this.PublicID = PublicID;
                this.UserID = UserID;
                this.BreadPointID = BreadPointID;
                this.PortionCount = PortionCount;
                this.Status = Status;
                this.Token = Token;
                this.CreatedAt = CreatedAt;
                this.ExpiresAt = ExpiresAt;
                this.ScannedAt = ScannedAt;
                this.IsScanned = IsScanned;
            }

            public int QRCodeID { get; set; }
            public Guid PublicID { get; set; }
            public int UserID { get; set; }
            public int BreadPointID { get; set; }
            public int PortionCount { get; set; }
            public int Status { get; set; }
            public byte[] Token { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ExpiresAt { get; set; }
            public DateTime? ScannedAt { get; set; }
            public bool IsScanned { get; set; }
        }

        /// <summary>
        /// Internal-use-only object (all fields nullable to support partial reads/updates).
        /// </summary>
        public class QrObjDTO
        {
            public QrObjDTO(int QRCodeID, Guid PublicID, int UserID,
                int BreadPointID, int PortionCount, int Status, byte[] Token
                , DateTime? ScannedAt, DateTime CreatedAt , DateTime ExpiresAt
                ,bool IsScanned)
            {
                this.QRCodeID = QRCodeID;
                this.PublicID = PublicID;
                this.UserID = UserID;
                this.BreadPointID = BreadPointID;
                this.PortionCount = PortionCount;
                this.Status = Status;
                this.ScannedAt = ScannedAt;
                this.Token = Token;
                this.CreatedAt = CreatedAt;
                this.ExpiresAt = ExpiresAt;
                this.IsScanned = IsScanned;

            }

            public int QRCodeID { get; set; }
            public Guid PublicID { get; set; }
            public int UserID { get; set; }
            public int BreadPointID { get; set; }
            public int PortionCount { get; set; }
            public int Status { get; set; }
            public byte[] Token { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ExpiresAt { get; set; }
            public bool IsScanned { get; set; }
            public DateTime? ScannedAt { get; set; }
        }
    }

    public class QRCodesData
    {
        // ---------- Mappers ----------

        /// <summary>
        /// Maps a full QR row into the DTO handed back to the front end.
        /// </summary>
        private static QRModel.QrInfoDTO MapToInfo(SqlDataReader reader)
        {
            return new QRModel.QrInfoDTO
            (
                QRCodeID: reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                UserID: reader.GetInt32(reader.GetOrdinal("UserID")),
                BreadPointID: reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                PortionCount: reader.GetInt32(reader.GetOrdinal("PortionCount")),
                Status: reader.GetInt32(reader.GetOrdinal("Status")),
                Token: reader.IsDBNull(reader.GetOrdinal("Token"))? null : (byte[])reader["Token"],
                CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                ExpiresAt: reader.GetDateTime(reader.GetOrdinal("ExpiresAt")),
                ScannedAt: reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt")),
                IsScanned: reader.GetBoolean(reader.GetOrdinal("IsScanned"))
            );
        }

        /// <summary>
        /// Maps a QR row into the internal-only object, tolerant of missing/null columns.
        /// Used for internal lookups (Find, Scan) that never get exposed to the front end.
        /// </summary>
        private static QRModel.QrObjDTO MapToObj(SqlDataReader reader)
        {
            var obj = new QRModel.QrObjDTO
            (
                QRCodeID: reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                UserID: reader.GetInt32(reader.GetOrdinal("UserID")),
                BreadPointID: reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                PortionCount: reader.GetInt32(reader.GetOrdinal("PortionCount")),
                Status: reader.GetInt32(reader.GetOrdinal("Status")),
                ScannedAt: reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt")),
                Token: reader.IsDBNull(reader.GetOrdinal("Token")) ? null : (byte[])reader["Token"],
                CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                ExpiresAt: reader.GetDateTime(reader.GetOrdinal("ExpiresAt")),
                IsScanned: reader.GetBoolean(reader.GetOrdinal("IsScanned"))

            );
            return obj;
        }

        // ---------- Front-facing reads (return InfoDTO) ----------

        public static QRModel.QrInfoDTO? GetOneQRCodeBy(
            int? QRCodeID = null,
            Guid? PublicID = null,
            byte[]? Token = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Token", Token ?? (object)DBNull.Value);
                var outputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    return MapToInfo(reader);
            }
            return null;
        }

        public static List<QRModel.QrInfoDTO> GetAllQRCodes(
            int? UserID = null,
            int? BreadPointID = null,
            int? Status = null,
            bool? IsScanned = null,
            int PageNumber = 1,
            int PageSize = 10)
        {
            var qrList = new List<QRModel.QrInfoDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", UserID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsScanned", IsScanned ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PageNumber", PageNumber < 1 ? 1 : PageNumber);
                cmd.Parameters.AddWithValue("@PageRow", PageSize < 1 ? 10 : PageSize);
                var outputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    qrList.Add(MapToInfo(reader));
            }
            return qrList;
        }

        // ---------- Internal reads (return ObjDTO) ----------

        /// <summary>
        /// Internal-only lookup — same underlying row as GetOneQRCodeBy, but returned as the
        /// nullable-everything ObjDTO for business logic (Find/Scan) that isn't allowed to
        /// leak straight to the front end.
        /// </summary>
        public static QRModel.QrObjDTO? GetQRCodeObjBy(
            int? QRCodeID = null,
            Guid? PublicID = null,
            byte[]? Token = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@QRCodeID", SqlDbType.Int)
                    .Value = QRCodeID ?? (object)DBNull.Value;

                cmd.Parameters.Add("@PublicID", SqlDbType.UniqueIdentifier)
                    .Value = PublicID ?? (object)DBNull.Value;

                cmd.Parameters.Add("@Token", SqlDbType.VarBinary, 255)
                    .Value = Token ?? (object)DBNull.Value; 
                var outputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    return MapToObj(reader);
            }
            return null;
        }

        // ---------- Writes ----------

        /// <summary>
        /// Takes what the front end sent to request a new QR code. Token/expiry/status are
        /// generated server-side (by the stored procedure), so they aren't accepted here.
        /// </summary>
        public static byte[] CreateQRCode(QRModel.QrObjDTO qrDTO, SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Create", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", qrDTO.UserID);
                cmd.Parameters.AddWithValue("@BreadPointID", qrDTO.BreadPointID);
                cmd.Parameters.AddWithValue("@PortionCount", qrDTO.PortionCount < 1 ? 1 : qrDTO.PortionCount);
                cmd.Parameters.AddWithValue("@PortionPrice", 3.50/*qrDTO.PortionPrice < 0 ? 3.5M : qrDTO.PortionPrice*/);
                var outputParam = new SqlParameter("@NewToken", SqlDbType.VarBinary , 32)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                SetSessionContext(conn, sessionInfo);
                cmd.ExecuteNonQuery();
                return (byte[])outputParam.Value;
            }
        }

        /// <summary>
        /// Internal-only partial update — takes the ObjDTO so callers can update just the
        /// fields they touched (e.g. Status/PortionCount/ExpiresAt) and leave the rest null.
        /// </summary>
        public static bool UpdateQRCode(QRModel.QrObjDTO qrCode, SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QRCodeID", qrCode.QRCodeID);
                cmd.Parameters.AddWithValue("@BreadPointID", qrCode.BreadPointID );
                cmd.Parameters.AddWithValue("@PortionCount", qrCode.PortionCount );
                cmd.Parameters.AddWithValue("@Status", qrCode.Status );
                cmd.Parameters.AddWithValue("@ExpiresAt", qrCode.ExpiresAt);

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

        /// <summary>
        /// Scans a QR code at a bread point. Returns the internal ObjDTO so the BL can inspect
        /// portion counts / status and react (e.g. adjust wallet balances) before deciding what,
        /// if anything, to hand back to the front end.
        /// </summary>
        public static QRModel.QrObjDTO? ScanQRCode(byte[] Token, SessionContextInfo sessionInfo, int BreadPointID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Scan", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Token", Token);
                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);

                conn.Open();
                SetSessionContext(conn, sessionInfo);
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    return MapToObj(reader);
            }
            return null;
        }

        public static bool DeleteQRCode(int QRCodeID, SessionContextInfo sessionInfo, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID);
                cmd.Parameters.AddWithValue("@HardDelete", HardDelete);

                var outputParam = new SqlParameter("@StatusCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                SetSessionContext(conn, sessionInfo);
                cmd.ExecuteNonQuery();
                return (int)outputParam.Value > 0;
            }
        }

        public static bool CancelQrCode(int QrCodeID, SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Cancel", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QRCodeID",QrCodeID);
                
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
        //-----------------------------//////////////////////////------------------------------
        public static void SetSessionContext(SqlConnection conn, SessionContextInfo sessionInfo)
        {
            using var cmd = new SqlCommand(@"
                 EXEC sp_set_session_context @key = N'UserID', @value = @UserID;
                 EXEC sp_set_session_context @key = N'Role', @value = @Role;
                 EXEC sp_set_session_context @key = N'IPAddress', @value = @IPAddress;
                 EXEC sp_set_session_context @key = N'RequestID', @value = @RequestID;", conn);

            cmd.Parameters.AddWithValue("@UserID", (object?)sessionInfo.UserID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Role", (object?)sessionInfo.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IPAddress", (object?)sessionInfo.IPAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RequestID", (object?)sessionInfo.RequestID ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }
}