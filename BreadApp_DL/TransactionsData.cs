using Microsoft.Data.SqlClient;
using System.Data;

namespace BreadApp_DL
{
    public class TransactionModel
    {
        public class TransactionDTO
        {
            public TransactionDTO(int? TransactionID, Guid? PublicID, int? SenderUserID,
                int? ReceiverUserID, int? BreadPointID, int? QRCodeID,
                decimal? Amount, int? TransactionType, int? Status,
                DateTime? CreatedAt, DateTime? ConfirmedAt, string? Notes)
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

            public int? TransactionID { get; set; }
            public Guid? PublicID { get; set; }
            public int? SenderUserID { get; set; }
            public int? ReceiverUserID { get; set; }
            public int? BreadPointID { get; set; }
            public int? QRCodeID { get; set; }
            public decimal? Amount { get; set; }
            public int? TransactionType { get; set; }
            public int? Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? ConfirmedAt { get; set; }
            public string? Notes { get; set; }
        }
        public class CreateTransactionDTO 
        {
            public CreateTransactionDTO(int? SenderUserID, int? ReceiverUserID, int? BreadPointID, int? QRCodeID, decimal? Amount, int? TransactionType, string? Notes)
            {
                this.SenderUserID = SenderUserID;
                this.ReceiverUserID = ReceiverUserID;
                this.BreadPointID = BreadPointID;
                this.QRCodeID = QRCodeID;
                this.Amount = Amount;
                this.TransactionType = TransactionType;
                this.Notes = Notes;
            }
            public int? SenderUserID { get; set; }
            public int? ReceiverUserID { get; set; }
            public int? BreadPointID { get; set; }
            public int? QRCodeID { get; set; }
            public decimal? Amount { get; set; }
            public int? TransactionType { get; set; }
            public string? Notes { get; set; }
        }
    }

    public class TransactionsData
    {
      
        private static TransactionModel.TransactionDTO MapRow(SqlDataReader reader)
        {
            return new TransactionModel.TransactionDTO
            (
                reader.IsDBNull(reader.GetOrdinal("TransactionID")) ? null : reader.GetInt32(reader.GetOrdinal("TransactionID")),
                reader.IsDBNull(reader.GetOrdinal("PublicID")) ? null : reader.GetGuid(reader.GetOrdinal("PublicID")),
                reader.IsDBNull(reader.GetOrdinal("SenderUserID")) ? null : reader.GetInt32(reader.GetOrdinal("SenderUserID")),
                reader.IsDBNull(reader.GetOrdinal("ReceiverUserID")) ? null : reader.GetInt32(reader.GetOrdinal("ReceiverUserID")),
                reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                reader.IsDBNull(reader.GetOrdinal("QRCodeID")) ? null : reader.GetInt32(reader.GetOrdinal("QRCodeID")),
                reader.IsDBNull(reader.GetOrdinal("Amount")) ? null : reader.GetDecimal(reader.GetOrdinal("Amount")),
                reader.IsDBNull(reader.GetOrdinal("TransactionType")) ? null : reader.GetInt32(reader.GetOrdinal("TransactionType")),
                reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetInt32(reader.GetOrdinal("Status")),
                reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                reader.IsDBNull(reader.GetOrdinal("ConfirmedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ConfirmedAt")),
                reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
            );
        }

     
        public static TransactionModel.TransactionDTO? GetTransactionBy(
            int? TransactionID = null,
            Guid? PublicID = null,
            int? SenderUserID = null,
            int? ReceiverUserID = null,
            int? BreadPointID = null,
            int? QRCodeID = null,
            string? TransactionType = null,
            string? Status = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Transactions_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TransactionID", TransactionID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SenderUserID", SenderUserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReceiverUserID", ReceiverUserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@QRCodeID", QRCodeID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TransactionType", TransactionType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                        return MapRow(reader);
                }
            }
            return null;
        }

       
        public static List<TransactionModel.TransactionDTO> GetTransactions(
            int? SenderUserID = null,
            int? BreadPointID = null,
            int? TransactionType = null,
            int? Status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var TransactionList = new List<TransactionModel.TransactionDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Transactions_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SenderUserID", SenderUserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TransactionType", TransactionType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                        TransactionList.Add(MapRow(reader));
                }
            }
            return TransactionList;
        }

        
        public static int CreateTransaction(TransactionModel.TransactionDTO transaction)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Transactions_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SenderUserID", transaction.SenderUserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReceiverUserID", transaction.ReceiverUserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BreadPointID", transaction.BreadPointID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@QRCodeID", transaction.QRCodeID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Amount", transaction.Amount ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", transaction.Notes ?? (object)DBNull.Value);
                    var OutputParam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OutputParam);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return (int)OutputParam.Value;
                }
            }
        }

     
        public static bool ConfirmTransaction(int TransactionID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString ))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Transactions_Confirm", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TransactionID", TransactionID);
                    var OutputParam = new SqlParameter("@StatusCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OutputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return (int)OutputParam.Value > 0;
                }
            }
        }

      
        public static bool UpdateTransaction(int TransactionID, int? Status = null, string? Notes = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Transactions_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TransactionID", TransactionID);
                    cmd.Parameters.AddWithValue("@Status", Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", Notes ?? (object)DBNull.Value);
                    var OutputParam = new SqlParameter("@StatusCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OutputParam);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return (int)OutputParam.Value > 0;
                }
            }
        }

       
        public static bool DeleteTransaction(int TransactionID, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Transactions_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TransactionID", TransactionID);
                    cmd.Parameters.AddWithValue("@HardDelete", HardDelete);
                    var OutputParam = new SqlParameter("@StatusCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OutputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return  (int)OutputParam.Value> 0;
                }
            }
        }
    }
}
