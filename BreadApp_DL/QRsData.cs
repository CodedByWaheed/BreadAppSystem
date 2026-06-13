using Microsoft.Data.SqlClient;
using System.Data;

namespace BreadApp_DL
{
    public class QRCodeModel
    {
        public class QRCodeDTO
        {
            public QRCodeDTO(int? QRCodeID, Guid? PublicID, int? UserID, int? BreadPointID,
                int? PortionCount, string? Status, string? Token,
                DateTime? CreatedAt, DateTime? ExpiresAt, DateTime? ScannedAt, bool? IsScanned)
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

            public int? QRCodeID { get; set; }
            public Guid? PublicID { get; set; }
            public int? UserID { get; set; }
            public int? BreadPointID { get; set; }
            public int? PortionCount { get; set; }
            public string? Status { get; set; }
            public string? Token { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? ExpiresAt { get; set; }
            public DateTime? ScannedAt { get; set; }
            public bool? IsScanned { get; set; }
        }

        public class QRScanResultDTO
        {
            public QRScanResultDTO(int? QRCodeID, Guid? PublicID, int? UserID,
                int? BreadPointID, int? PortionCount, string? Status, DateTime? ScannedAt)
            {
                this.QRCodeID = QRCodeID;
                this.PublicID = PublicID;
                this.UserID = UserID;
                this.BreadPointID = BreadPointID;
                this.PortionCount = PortionCount;
                this.Status = Status;
                this.ScannedAt = ScannedAt;
            }

            public int? QRCodeID { get; set; }
            public Guid? PublicID { get; set; }
            public int? UserID { get; set; }
            public int? BreadPointID { get; set; }
            public int? PortionCount { get; set; }
            public string? Status { get; set; }
            public DateTime? ScannedAt { get; set; }
        }

    }

    public class QRCodesData
    {
        private static QRCodeModel.QRCodeDTO MapRow(SqlDataReader reader)
        {
            return new QRCodeModel.QRCodeDTO
            (
                reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                reader.IsDBNull(reader.GetOrdinal("PublicID")) ? null : reader.GetGuid(reader.GetOrdinal("PublicID")),
                reader.IsDBNull(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
                reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                reader.IsDBNull(reader.GetOrdinal("PortionCount")) ? null : reader.GetInt32(reader.GetOrdinal("PortionCount")),
                reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                reader.IsDBNull(reader.GetOrdinal("Token")) ? null : reader.GetString(reader.GetOrdinal("Token")),
                reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                reader.IsDBNull(reader.GetOrdinal("ExpiresAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ExpiresAt")),
                reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt")),
                reader.IsDBNull(reader.GetOrdinal("IsScanned")) ? null : reader.GetBoolean(reader.GetOrdinal("IsScanned"))
            );
        }

     
        public static QRCodeModel.QRCodeDTO? GetQRCodeBy(
            int? QRCodeID = null,
            Guid? PublicID = null,
            int? UserID = null,
            int? BreadPointID = null,
            string? Token = null,
            string? Status = null,
            bool? IsScanned = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserID", UserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Token", Token ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsScanned", IsScanned ?? (object)DBNull.Value);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                        return MapRow(reader);
                }
            }
            return null;
        }

   
        public static List<QRCodeModel.QRCodeDTO> GetQRCodes(
            int? UserID = null,
            int? BreadPointID = null,
            string? Status = null,
            bool? IsScanned = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var QRList = new List<QRCodeModel.QRCodeDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsScanned", IsScanned ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                        QRList.Add(MapRow(reader));
                }
            }
            return QRList;
        }

        public static int CreateQRCode(int UserID, int BreadPointID, int PortionCount = 1, DateTime? ExpiresAt = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);
                    cmd.Parameters.AddWithValue("@PortionCount", PortionCount < 1 ? 1 : PortionCount);
                    cmd.Parameters.AddWithValue("@ExpiresAt", ExpiresAt ?? (object)DBNull.Value);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static bool UpdateQRCode(QRCodeModel.QRCodeDTO qrCode)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@QRCodeID", qrCode.QRCodeID < 1 ? (object)DBNull.Value : qrCode.QRCodeID);
                    cmd.Parameters.AddWithValue("@BreadPointID", qrCode.BreadPointID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PortionCount", qrCode.PortionCount ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", qrCode.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ExpiresAt", qrCode.ExpiresAt ?? (object)DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

       
        public static QRCodeModel.QRScanResultDTO? ScanQRCode(string Token, int BreadPointID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Scan", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Token", Token);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new QRCodeModel.QRScanResultDTO
                        (
                            reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                            reader.IsDBNull(reader.GetOrdinal("PublicID")) ? null : reader.GetGuid(reader.GetOrdinal("PublicID")),
                            reader.IsDBNull(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
                            reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                            reader.IsDBNull(reader.GetOrdinal("PortionCount")) ? null : reader.GetInt32(reader.GetOrdinal("PortionCount")),
                            reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                            reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt"))
                        );
                    }
                }
            }
            return null;
        }

        public static bool DeleteQRCode(int QRCodeID, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_QRCodes_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID);
                    cmd.Parameters.AddWithValue("@HardDelete", HardDelete);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
