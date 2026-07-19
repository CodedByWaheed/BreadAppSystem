//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace BreadApp_DL
//{
//    public class QRModel
//    {
//        //this the Data Come from the Front
//        public class QrDataDTO
//        {
//            public QrDataDTO(int UserID,int BreadPointID, int PortionCount)
//            {

//                this.UserID = UserID;
//                this.BreadPointID = BreadPointID;
//                this.PortionCount = PortionCount;
//            }

//            public int UserID { get; set; }
//            public int BreadPointID { get; set; }
//            public int PortionCount { get; set; }

//        }
//        // this is the data that i gave to the front
//        public class QrInfoDTO
//        {
//            public QrInfoDTO(Guid PublicID, int UserID, int BreadPointID,
//                int PortionCount, int Status, string Token,
//                DateTime CreatedAt, DateTime ExpiresAt, DateTime? ScannedAt, bool IsScanned)
//            {

//                this.PublicID = PublicID;
//                this.UserID = UserID;
//                this.BreadPointID = BreadPointID;
//                this.PortionCount = PortionCount;
//                this.Status = Status;
//                this.Token = Token;
//                this.CreatedAt = CreatedAt;
//                this.ExpiresAt = ExpiresAt;
//                this.ScannedAt = ScannedAt;
//                this.IsScanned = IsScanned;
//            }

//            public int QRCodeID { get; set; }
//            public Guid PublicID { get; set; }
//            public int UserID { get; set; }
//            public int BreadPointID { get; set; }
//            public int PortionCount { get; set; }
//            public int Status { get; set; }
//            public string Token { get; set; }
//            public DateTime CreatedAt { get; set; }
//            public DateTime ExpiresAt { get; set; }
//            public DateTime? ScannedAt { get; set; }
//            public bool IsScanned { get; set; }
//        }
//        // this is for internal use 
//        public class QrObjDTO
//        {
//            public QrObjDTO(int? QRCodeID, Guid? PublicID, int? UserID,
//                int? BreadPointID, int? PortionCount, string? Status, DateTime? ScannedAt)
//            {
//                this.QRCodeID = QRCodeID;
//                this.PublicID = PublicID;
//                this.UserID = UserID;
//                this.BreadPointID = BreadPointID;
//                this.PortionCount = PortionCount;
//                this.Status = Status;
//                this.ScannedAt = ScannedAt;
//            }

//            public int? QRCodeID { get; set; }
//            public Guid? PublicID { get; set; }
//            public int? UserID { get; set; }
//            public int? BreadPointID { get; set; }
//            public int? PortionCount { get; set; }
//            public string? Status { get; set; }
//            public string? Token { get; set; }
//            public DateTime? CreatedAt { get; set; }
//            public DateTime? ExpiresAt { get; set; }
//            public bool? IsScanned { get; set; }
//            public DateTime? ScannedAt { get; set; }
//        }

//    }

//    public class QRCodesData
//    {
//        private static QRCodeModel.QRCodeDTO MapRow(SqlDataReader reader)
//        {
//            return new QRCodeModel.QRCodeDTO
//            (
//                reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
//                reader.IsDBNull(reader.GetOrdinal("PublicID")) ? null : reader.GetGuid(reader.GetOrdinal("PublicID")),
//                reader.IsDBNull(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
//                reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
//                reader.IsDBNull(reader.GetOrdinal("PortionCount")) ? null : reader.GetInt32(reader.GetOrdinal("PortionCount")),
//                reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetInt32(reader.GetOrdinal("Status")),
//                reader.IsDBNull(reader.GetOrdinal("Token")) ? null : reader.GetString(reader.GetOrdinal("Token")),
//                reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
//                reader.IsDBNull(reader.GetOrdinal("ExpiresAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ExpiresAt")),
//                reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt")),
//                reader.IsDBNull(reader.GetOrdinal("IsScanned")) ? null : reader.GetBoolean(reader.GetOrdinal("IsScanned"))
//            );
//        }


//        public static QRCodeModel.QRCodeDTO? GetOneQRCodeBy(
//            int? QRCodeID = null,
//            Guid? PublicID = null,
//            string? Token = null

//            )
//        {
//            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@Token", Token ?? (object)DBNull.Value);

//                    conn.Open();
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    if (reader.Read())
//                        return MapRow(reader);
//                }
//            }
//            return null;
//        }


//        public static List<QRCodeModel.QRCodeDTO> GetAllQRCodes(

//            int? UserID = null,
//            int? BreadPointID = null,
//            string? Status = null,
//            bool? IsScanned = null,
//            int PageNumber = 1,
//            int PageSize = 10)
//        {
//            var QRList = new List<QRCodeModel.QRCodeDTO>();

//            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    cmd.Parameters.AddWithValue("@UserID", UserID ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@IsScanned", IsScanned ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber < 1 ? 1 : PageNumber);
//                    cmd.Parameters.AddWithValue("@PageRow", PageSize < 1 ? 10 : PageSize);

//                    conn.Open();
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    while (reader.Read())
//                        QRList.Add(MapRow(reader));
//                }
//            }
//            return QRList;
//        }

//        public static int CreateQRCode(QRCodeModel.CreateQRDTO ctreatQrDTO)
//        {
//            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Create", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    cmd.Parameters.AddWithValue("@UserID", ctreatQrDTO.UserID);
//                    cmd.Parameters.AddWithValue("@BreadPointID", ctreatQrDTO.BreadPointID);
//                    cmd.Parameters.AddWithValue("@PortionCount", ctreatQrDTO.PortionCount < 1 ? 1 : ctreatQrDTO.PortionCount);
//                    cmd.Parameters.AddWithValue("@ExpiresAt", ctreatQrDTO.ExpiresAt ?? (object)DBNull.Value);
//                    var outputParam = new SqlParameter("@NewID", SqlDbType.Int)
//                    {
//                        Direction = ParameterDirection.Output
//                    };
//                    cmd.Parameters.Add(outputParam);
//                    conn.Open();
//                    cmd.ExecuteNonQuery();
//                    return (int)outputParam.Value;
//                }
//            }
//        }

//        public static bool UpdateQRCode(QRCodeModel.QRCodeDTO qrCode)
//        {
//            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Update", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    cmd.Parameters.AddWithValue("@QRCodeID", qrCode.QRCodeID < 1 ? (object)DBNull.Value : qrCode.QRCodeID);
//                    cmd.Parameters.AddWithValue("@BreadPointID", qrCode.BreadPointID ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@PortionCount", qrCode.PortionCount ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@Status", qrCode.Status ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@ExpiresAt", qrCode.ExpiresAt ?? (object)DBNull.Value);
//                    var outputParam = new SqlParameter("@StatusCode", SqlDbType.Int)
//                    {
//                        Direction = ParameterDirection.Output
//                    };
//                    cmd.Parameters.Add(outputParam);

//                    conn.Open();
//                    cmd.ExecuteNonQuery();
//                    return (int)outputParam.Value > 0;
//                }
//            }
//        }


//        public static QRCodeModel.QRScanResultDTO? ScanQRCode(string Token, int BreadPointID)
//        {
//            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Scan", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    cmd.Parameters.AddWithValue("@Token", Token);
//                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);

//                    conn.Open();
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    if (reader.Read())
//                    {
//                        return new QRCodeModel.QRScanResultDTO
//                        (
//                            reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
//                            reader.IsDBNull(reader.GetOrdinal("PublicID")) ? null : reader.GetGuid(reader.GetOrdinal("PublicID")),
//                            reader.IsDBNull(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
//                            reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
//                            reader.IsDBNull(reader.GetOrdinal("PortionCount")) ? null : reader.GetInt32(reader.GetOrdinal("PortionCount")),
//                            reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
//                            reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt"))
//                        );
//                    }
//                }
//            }
//            return null;
//        }

//        public static bool DeleteQRCode(int QRCodeID, bool HardDelete = false)
//        {
//            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Delete", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID);
//                    cmd.Parameters.AddWithValue("@HardDelete", HardDelete);
//                    var outputParam = new SqlParameter("@StatusCode", SqlDbType.Int)
//                    {
//                        Direction = ParameterDirection.Output
//                    };
//                    cmd.Parameters.Add(outputParam);

//                    conn.Open();
//                    cmd.ExecuteNonQuery();
//                    return (int)outputParam.Value>0;
//                }
//            }
//        }
//    }
//}

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
            public QrDataDTO(int UserID, int BreadPointID, int PortionCount)
            {
                this.UserID = UserID;
                this.BreadPointID = BreadPointID;
                this.PortionCount = PortionCount;
            }

            public int UserID { get; set; }
            public int BreadPointID { get; set; }
            public int PortionCount { get; set; }
        }

        /// <summary>
        /// Data we give back TO the front end.
        /// </summary>
        public class QrInfoDTO
        {
            public QrInfoDTO(int QRCodeID, Guid PublicID, int UserID, int BreadPointID,
                int PortionCount, int Status, string Token,
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
            public string Token { get; set; }
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
                int BreadPointID, int PortionCount, int Status, DateTime? ScannedAt)
            {
                this.QRCodeID = QRCodeID;
                this.PublicID = PublicID;
                this.UserID = UserID;
                this.BreadPointID = BreadPointID;
                this.PortionCount = PortionCount;
                this.Status = Status;
                this.ScannedAt = ScannedAt;
            }

            public int QRCodeID { get; set; }
            public Guid PublicID { get; set; }
            public int UserID { get; set; }
            public int BreadPointID { get; set; }
            public int PortionCount { get; set; }
            public int Status { get; set; }
            public string Token { get; set; }
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
                Token: reader.GetString(reader.GetOrdinal("Token")),
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
                ScannedAt: reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt"))
            );

            obj.Token = reader.GetString(reader.GetOrdinal("Token"));
            obj.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            obj.ExpiresAt = reader.GetDateTime(reader.GetOrdinal("ExpiresAt"));
            obj.IsScanned = reader.GetBoolean(reader.GetOrdinal("IsScanned"));

            return obj;
        }

        // ---------- Front-facing reads (return InfoDTO) ----------

        public static QRModel.QrInfoDTO? GetOneQRCodeBy(
            int? QRCodeID = null,
            Guid? PublicID = null,
            string? Token = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Token", Token ?? (object)DBNull.Value);

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
            string? Token = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Token", Token ?? (object)DBNull.Value);

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
        public static string CreateQRCode(QRModel.QrDataDTO qrDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Create", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", qrDTO.UserID);
                cmd.Parameters.AddWithValue("@BreadPointID", qrDTO.BreadPointID);
                cmd.Parameters.AddWithValue("@PortionCount", qrDTO.PortionCount < 1 ? 1 : qrDTO.PortionCount);

                var outputParam = new SqlParameter("@Token", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (string)outputParam.Value;
            }
        }

        /// <summary>
        /// Internal-only partial update — takes the ObjDTO so callers can update just the
        /// fields they touched (e.g. Status/PortionCount/ExpiresAt) and leave the rest null.
        /// </summary>
        public static bool UpdateQRCode(QRModel.QrObjDTO qrCode)
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
                cmd.Parameters.AddWithValue("@ScannedAt", qrCode.ScannedAt ?? (object)DBNull.Value);

                var outputParam = new SqlParameter("@StatusCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputParam.Value > 0;
            }
        }

        /// <summary>
        /// Scans a QR code at a bread point. Returns the internal ObjDTO so the BL can inspect
        /// portion counts / status and react (e.g. adjust wallet balances) before deciding what,
        /// if anything, to hand back to the front end.
        /// </summary>
        public static QRModel.QrObjDTO? ScanQRCode(string Token, int BreadPointID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Scan", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Token", Token);
                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    return MapToObj(reader);
            }
            return null;
        }

        public static bool DeleteQRCode(int QRCodeID, bool HardDelete = false)
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
                cmd.ExecuteNonQuery();
                return (int)outputParam.Value > 0;
            }
        }
    }
}