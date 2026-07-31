using BreadApp_Struct.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BreadApp_DL
{
    public class TransactionModel
    {
        /// <summary>
        /// Data coming FROM the front end (create-a-transaction request).
        /// Note: the front supplies a QR "token" reference (QRToken), not the resolved
        /// internal QRCodeID — the create stored procedure is expected to resolve it.
        /// </summary>
        public class TransactionDataDTO
        {


            public int? SenderUserID { get; set; } = null;
            public int? ReceiverUserID { get; set; } = null;
            public int? BreadPointID { get; set; } = null;
            public int? QrID { get; set; } = null;
            public decimal? Amount { get; set; } = null;
            public int? TransactionType { get; set; } = null;
            public string? Notes { get; set; } = null;
        }

        /// <summary>
        /// Data returned TO the front end. No internal auto-increment TransactionID —
        /// PublicID is what the front should reference.
        /// </summary>
        public class TransactionInfoDTO
        {
            public TransactionInfoDTO(int TransactionID ,Guid PublicID, int? SenderUserID,
                int? ReceiverUserID, int? BreadPointID, int? QRCodeID,
                decimal Amount, int TransactionType, int Status, DateTime? ConfirmedAt, string Notes)
            {
                this.TransactionID = TransactionID;
                this.PublicID = PublicID;
                this.SenderUserID = SenderUserID;
                this.ReceiverUserID = ReceiverUserID;
                this.BreadPointID = BreadPointID;
                this.QRCodeID = QRCodeID;
                this.Amount = Amount;
                this.TransactionType = TransactionType;
                this.Status = Status;
                this.ConfirmedAt = ConfirmedAt;
                this.Notes = Notes;
            }

            public int TransactionID { get; set; }
            public Guid PublicID { get; set; }
            public int? SenderUserID { get; set; }
            public int? ReceiverUserID { get; set; }
            public int? BreadPointID { get; set; }
            public int? QRCodeID { get; set; }
            public decimal Amount { get; set; }
            public int TransactionType { get; set; }
            public int Status { get; set; }
            public DateTime? ConfirmedAt { get; set; }
            public string? Notes { get; set; }
        }

        public class TransactionUserInfoDTO
        {
            public TransactionUserInfoDTO(int TransactionID, Guid PublicID, string SenderUsername,
                string? ReceiverUsername, string? BreadPointName, int? QRCodeID,
                decimal Amount, string TransactionType, string Status, DateTime? ConfirmedAt, string Notes)
            {
                this.TransactionID = TransactionID;
                this.PublicID = PublicID;
                this.SenderName = SenderUsername;
                this.ReceiverName = ReceiverUsername;
                this.BreadPointName = BreadPointName;
                this.QRCodeID = QRCodeID;
                this.Amount = Amount;
                this.TransactionType = TransactionType;
                this.Status = Status;
                this.ConfirmedAt = ConfirmedAt;
                this.Notes = Notes;
            }

            public int TransactionID { get; set; }
            public Guid PublicID { get; set; }
            public string? SenderName { get; set; }
            public string? ReceiverName { get; set; }
            public string? BreadPointName { get; set; }
            public int? QRCodeID { get; set; }
            public decimal Amount { get; set; }
            public string TransactionType { get; set; }
            public string Status { get; set; }
            public DateTime? ConfirmedAt { get; set; }
            public string? Notes { get; set; }
        }

        /// <summary>
        /// Full internal object — includes TransactionID/CreatedAt, used for Find/Confirm/Update/Delete.
        /// Never returned from a controller.
        /// </summary>
        public class TransactionObjDTO
        {
            public TransactionObjDTO(int TransactionID, Guid PublicID, int? SenderUserID,
                int? ReceiverUserID, int? BreadPointID, int? QRCodeID,
                decimal Amount, int TransactionType, int Status, DateTime CreatedAt, DateTime? ConfirmedAt, string Notes)
            {
                this.TransactionID = TransactionID;
                this.PublicID = PublicID;
                this.SenderUserID = SenderUserID;
                this.ReceiverUserID = ReceiverUserID;
                this.BreadPointID = BreadPointID;
                this.QRCodeID = QRCodeID;
                this.Amount = Amount;
                this.TransactionType = TransactionType;
                this.Status = Status;
                this.CreatedAt = CreatedAt;
                this.ConfirmedAt = ConfirmedAt;
                this.Notes = Notes;
            }

            public int TransactionID { get; set; }
            public Guid PublicID { get; set; }
            public int? SenderUserID { get; set; }
            public int? ReceiverUserID { get; set; }
            public int? BreadPointID { get; set; }
            public int? QRCodeID { get; set; }
            public decimal Amount { get; set; }
            public int TransactionType { get; set; }
            public int Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? ConfirmedAt { get; set; }
            public string? Notes { get; set; }
        }
    }

    public class TransactionsData
    {
        // ---------- Mappers ----------

        private static TransactionModel.TransactionInfoDTO MapToInfo(SqlDataReader reader)
        {
            return new TransactionModel.TransactionInfoDTO
            (
                TransactionID: reader.GetInt32(reader.GetOrdinal("TransactionID")),
                PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                SenderUserID: reader.GetInt32(reader.GetOrdinal("SenderUserID")),
                ReceiverUserID: reader.IsDBNull(reader.GetOrdinal("ReceiverUserID")) ? null : reader.GetInt32(reader.GetOrdinal("ReceiverUserID")),
                BreadPointID: reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                QRCodeID: reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                Amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                TransactionType: reader.GetInt32(reader.GetOrdinal("TransactionType")),
                Status: reader.GetInt32(reader.GetOrdinal("Status")),
                ConfirmedAt: reader.IsDBNull(reader.GetOrdinal("ConfirmedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ConfirmedAt")),
                Notes: reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
            );
        }
        private static TransactionModel.TransactionUserInfoDTO MapToUserInfo(SqlDataReader reader)
        {
            return new TransactionModel.TransactionUserInfoDTO
            (
                TransactionID: reader.GetInt32(reader.GetOrdinal("TransactionID")),
                PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                SenderUsername: reader.GetString(reader.GetOrdinal("SenderName")),
                ReceiverUsername: reader.IsDBNull(reader.GetOrdinal("ReceiverName")) ? null : reader.GetString(reader.GetOrdinal("ReceiverName")),
                BreadPointName: reader.IsDBNull(reader.GetOrdinal("BreadPointName")) ? null : reader.GetString(reader.GetOrdinal("BreadPointName")),
                QRCodeID: reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                Amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                TransactionType: reader.GetString(reader.GetOrdinal("TransactionType")),
                Status: reader.GetString(reader.GetOrdinal("Status")),
                ConfirmedAt: reader.IsDBNull(reader.GetOrdinal("ConfirmedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ConfirmedAt")),
                Notes: reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
            );
        }

        private static TransactionModel.TransactionObjDTO MapToObj(SqlDataReader reader)
        {
            return new TransactionModel.TransactionObjDTO
            (
                TransactionID: reader.GetInt32(reader.GetOrdinal("TransactionID")),
                PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                SenderUserID: reader.GetInt32(reader.GetOrdinal("SenderUserID")),
                ReceiverUserID: reader.IsDBNull(reader.GetOrdinal("ReceiverUserID")) ? null : reader.GetInt32(reader.GetOrdinal("ReceiverUserID")),
                BreadPointID: reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                QRCodeID: reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                Amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                TransactionType: reader.GetInt32(reader.GetOrdinal("TransactionType")),
                Status: reader.GetInt32(reader.GetOrdinal("Status")),
                CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                ConfirmedAt: reader.IsDBNull(reader.GetOrdinal("ConfirmedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ConfirmedAt")),
                Notes: reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
            );
        }

        // ---------- Front-facing reads (return InfoDTO) ----------

        public static TransactionModel.TransactionInfoDTO? GetTransactionBy(
            int? TransactionID = null,
            Guid? PublicID = null,
            int? QRCodeID = null
          )
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionID", TransactionID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
                var outputParam = new SqlParameter("@RecordsCount", SqlDbType.Int)
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

        public static List<TransactionModel.TransactionInfoDTO> GetTransactions(
            int? SenderUserID = null,
            int? ReceiverUserID = null ,
            int? BreadPointID = null,
            int? TransactionType = null,
            int? Status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var transactionList = new List<TransactionModel.TransactionInfoDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SenderUserID", SenderUserID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ReceiverUserID", ReceiverUserID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TransactionType", TransactionType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);
                var outputParam = new SqlParameter("@RecordsCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);
                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    transactionList.Add(MapToInfo(reader));
            }
            return transactionList;
        }
        public static List<TransactionModel.TransactionUserInfoDTO> GetTransactionsUserInfo(
           int? SenderUserID = null,
           int? ReceiverUserID = null,
           int? BreadPointID = null,
           int? TransactionType = null,
           int? Status = null,
           int pageNumber = 1,
           int pageSize = 10)
        {
            var transactionList = new List<TransactionModel.TransactionUserInfoDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_GetInfo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SenderUserID", SenderUserID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ReceiverUserID", ReceiverUserID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TransactionType", TransactionType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);
                var outputParam = new SqlParameter("@RecordsCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);
                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    transactionList.Add(MapToUserInfo(reader));
            }
            return transactionList;
        }

        // ---------- Internal reads (return ObjDTO) ----------

        /// <summary>
        /// Internal-only lookup — same row as GetTransactionBy, but with TransactionID/CreatedAt
        /// included so the BL can Confirm/Update/Delete afterward.
        /// </summary>
        public static TransactionModel.TransactionObjDTO? GetTransactionObjBy(
            int? TransactionID = null,
            Guid? PublicID = null,
            int? QRCodeID = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionID", TransactionID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
                var outputParam = new SqlParameter("@RecordsCount", SqlDbType.Int)
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
        /// Takes what the front sent to create a transaction. QRToken is passed through as-is;
        /// the stored procedure is expected to resolve it to the actual QRCodeID.
        /// </summary>
        public static int CreateTransaction(TransactionModel.TransactionObjDTO transaction, SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_Create", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SenderUserID", transaction.SenderUserID);
                cmd.Parameters.AddWithValue("@ReceiverUserID", transaction.ReceiverUserID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BreadPointID", transaction.BreadPointID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@QrID", transaction.QRCodeID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
                cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                cmd.Parameters.AddWithValue("@Notes", transaction.Notes ?? (object)DBNull.Value);

                var outputParam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                SetSessionContext(conn, sessionInfo);
                cmd.ExecuteNonQuery();
                return (int)outputParam.Value; ;
            }
        }

        public static bool ConfirmTransaction(int TransactionID, SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_Confirm", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionID", TransactionID);

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

        public static bool UpdateTransaction(int TransactionID, SessionContextInfo sessionInfo, int? Status = null, string? Notes = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionID", TransactionID);
                cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Notes", Notes ?? (object)DBNull.Value);

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

        public static bool DeleteTransaction(int TransactionID, SessionContextInfo sessionInfo, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Transactions_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionID", TransactionID);
                cmd.Parameters.AddWithValue("@HardDelete", HardDelete);

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