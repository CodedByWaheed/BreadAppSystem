using Microsoft.Data.SqlClient;
using System.Data;

namespace BreadApp_DL
{
    public class BreadPointModel
    {
        public class BreadPointDTO
        {
            public BreadPointDTO(int? BreadPointID, Guid? PublicID, string? Name, string? Address,
                string? PhoneNumber, int? AvailablePortions, Decimal? WalletBalance,
                double? Latitude, double? Longitude, bool? IsActive, DateTime? CreatedAt)
            {
                this.BreadPointID = BreadPointID;
                this.PublicID = PublicID;
                this.Name = Name;
                this.Address = Address;
                this.PhoneNumber = PhoneNumber;
                this.AvailablePortions = AvailablePortions;
                this.WalletBalance = WalletBalance;
                this.Latitude = Latitude;
                this.Longitude = Longitude;
                this.IsActive = IsActive;
                this.CreatedAt = CreatedAt;
            }

            public int? BreadPointID { get; set; }
            public Guid? PublicID { get; set; }
            public string? Name { get; set; }
            public string? Address { get; set; }
            public string? PhoneNumber { get; set; }
            public int? AvailablePortions { get; set; }
            public Decimal? WalletBalance { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public bool? IsActive { get; set; }
            public DateTime? CreatedAt { get; set; }
        }
    }

    public class BreadPointsData
    {
       
        private static BreadPointModel.BreadPointDTO MapRow(SqlDataReader reader)
        {
            return new BreadPointModel.BreadPointDTO
            (
                reader.IsDBNull(reader.GetOrdinal("BreadPointID")) ? null : reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                reader.IsDBNull(reader.GetOrdinal("PublicID")) ? null : reader.GetGuid(reader.GetOrdinal("PublicID")),
                reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                reader.IsDBNull(reader.GetOrdinal("AvailablePortions")) ? null : reader.GetInt32(reader.GetOrdinal("AvailablePortions")),
                reader.IsDBNull(reader.GetOrdinal("WalletBalance")) ? null : reader.GetDecimal(reader.GetOrdinal("WalletBalance")),
                reader.IsDBNull(reader.GetOrdinal("Latitude")) ? null : reader.GetDouble(reader.GetOrdinal("Latitude")),
                reader.IsDBNull(reader.GetOrdinal("Longitude")) ? null : reader.GetDouble(reader.GetOrdinal("Longitude")),
                reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive")),
                reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        public static BreadPointModel.BreadPointDTO? GetBreadPointBy(
            int? BreadPointID = null,
            Guid? PublicID = null,
            string? Name = null,
            bool? IsActive = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive ?? (object)DBNull.Value);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                        return MapRow(reader);
                }
            }
            return null;
        }

      
        public static List<BreadPointModel.BreadPointDTO> GetBreadPoints(
            bool? IsActive = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var BreadPointList = new List<BreadPointModel.BreadPointDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IsActive", IsActive ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                        BreadPointList.Add(MapRow(reader));
                }
            }
            return BreadPointList;
        }

       
        public static int CreateBreadPoint(BreadPointModel.BreadPointDTO breadPoint)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", breadPoint.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", breadPoint.Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PhoneNumber", breadPoint.PhoneNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AvailablePortions", breadPoint.AvailablePortions ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@WalletBalance", breadPoint.WalletBalance ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Latitude", breadPoint.Latitude ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitude", breadPoint.Longitude ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", true);
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

      
        public static bool UpdateBreadPoint(BreadPointModel.BreadPointDTO breadPoint)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BreadPointID", breadPoint.BreadPointID < 1 ? (object)DBNull.Value : breadPoint.BreadPointID);
                    cmd.Parameters.AddWithValue("@Name", breadPoint.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", breadPoint.Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PhoneNumber", breadPoint.PhoneNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AvailablePortions", breadPoint.AvailablePortions ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@WalletBalance", breadPoint.WalletBalance ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Latitude", breadPoint.Latitude ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitude", breadPoint.Longitude ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", breadPoint.IsActive ?? (object)DBNull.Value);
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

      
        public static bool DeleteBreadPoint(int BreadPointID, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);
                    cmd.Parameters.AddWithValue("@HardDelete", HardDelete);
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
    }
}