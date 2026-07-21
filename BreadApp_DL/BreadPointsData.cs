
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BreadApp_DL
{
    public class BreadPointModel
    {
        /// <summary>
        /// Data coming FROM the front end (create / update requests).
        /// </summary>
        public class BreadPointDataDTO : UserModel.UserDataDTO
        {


            public int? BreadPointID { get; set; } = null;
            public int? UserID { get; set; } = null;
            public string? Name { get; set; } = null;
            public string? Address { get; set; } = null;
            public string? PhoneNumber { get; set; } = null;
            public int? AvailablePortions { get; set; } = null;
            
        }

        /// <summary>
        /// Data returned TO the front end (read/list results). No sensitive/internal fields.
        /// </summary>
        public class BreadPointInfoDTO
        {
            public BreadPointInfoDTO(int BreadPointID, Guid PublicID, int UserID, string Name, string Address,
                string PhoneNumber, int AvailablePortions, decimal WalletBalance,
                double? Latitude, double? Longitude, bool IsActive, DateTime CreatedAt)
            {
                this.BreadPointID = BreadPointID;
                this.PublicID = PublicID;
                this.UserID = UserID;
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

            public int BreadPointID { get; set; }
            public Guid PublicID { get; set; }
            public int UserID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public int AvailablePortions { get; set; }
            public decimal WalletBalance { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        /// <summary>
        /// Full internal object (BreadPoint + owning User), used only inside the program
        /// (business logic, auth/ownership checks, wallet operations, etc.). Never sent to the front end.
        /// </summary>
        public class BreadPointObjDTO : UserModel.UserObjDTO
        {
            public BreadPointObjDTO(int breadPointID, Guid BPpublicID, int userID
                , string name, string address, string phoneNumber, int availablePortions
                , decimal BPwalletBalance, double? latitude, double? longitude, bool BPisActive
                , DateTime BPcreatedAt) : base(userID , Guid.Empty , string.Empty , string.Empty , string.Empty ,
                    DateTime.MinValue , false , null , string.Empty , 0M , null , null , false , DateTime.Now ,
                    string.Empty , string.Empty , string.Empty , null , null ,null)
            {
                this.BreadPointID = breadPointID;
                this.UserID = userID;
                this.BPPublicID = BPpublicID;
                this.Name = name;
                this.Address = address;
                this.PhoneNumber = phoneNumber;
                this.AvailablePortions = availablePortions;
                this.BPWalletBalance = BPwalletBalance;
                this.Latitude = latitude;
                this.Longitude = longitude;
                this.BPIsActive = BPisActive;
                this.BPCreatedAt = BPcreatedAt;
            }

            public int BreadPointID { get; set; }
            public Guid BPPublicID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public int AvailablePortions { get; set; }
            public decimal BPWalletBalance { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public bool BPIsActive { get; set; }
            public DateTime BPCreatedAt { get; set; }
        }
    }

    public class BreadPointsData
    {
        // ---------- Mappers ----------

        /// <summary>
        /// Maps a row from a BreadPoint-only result set into the DTO handed back to the front end.
        /// Requires the reader to include a UserID column alongside the usual BreadPoint columns.
        /// </summary>
        private static BreadPointModel.BreadPointInfoDTO MapToInfo(SqlDataReader reader)
        {
            return new BreadPointModel.BreadPointInfoDTO
            (
                BreadPointID: reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                UserID: reader.GetInt32(reader.GetOrdinal("UserID")),
                Name: reader.GetString(reader.GetOrdinal("Name")),
                Address: reader.GetString(reader.GetOrdinal("Address")),
                PhoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                AvailablePortions: reader.GetInt32(reader.GetOrdinal("AvailablePortions")),
                WalletBalance: reader.GetDecimal(reader.GetOrdinal("WalletBalance")),
                Latitude: reader.IsDBNull(reader.GetOrdinal("Latitude")) ? null : reader.GetDouble(reader.GetOrdinal("Latitude")),
                Longitude: reader.IsDBNull(reader.GetOrdinal("Longitude")) ? null : reader.GetDouble(reader.GetOrdinal("Longitude")),
                IsActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }

        /// <summary>
        /// Maps a row from a BreadPoint JOIN Users result set into the full internal object.
        /// Used only by internal callers (e.g. ownership/auth checks), never exposed to the front end.
        /// </summary>
        private static BreadPointModel.BreadPointObjDTO MapToObj(SqlDataReader reader)
        {
            return new BreadPointModel.BreadPointObjDTO
            (
                breadPointID: reader.GetInt32(reader.GetOrdinal("BreadPointID")),
                BPpublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                userID: reader.GetInt32(reader.GetOrdinal("UserID")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                address: reader.GetString(reader.GetOrdinal("Address")),
                phoneNumber: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                availablePortions: reader.GetInt32(reader.GetOrdinal("AvailablePortions")),
                BPwalletBalance: reader.GetDecimal(reader.GetOrdinal("WalletBalance")),
                latitude: reader.IsDBNull(reader.GetOrdinal("Latitude")) ? null : reader.GetDouble(reader.GetOrdinal("Latitude")),
                longitude: reader.IsDBNull(reader.GetOrdinal("Longitude")) ? null : reader.GetDouble(reader.GetOrdinal("Longitude")),
                BPisActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                BPcreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                );
        }

        // ---------- Front-facing reads (return InfoDTO) ----------

        public static BreadPointModel.BreadPointInfoDTO? GetBreadPointBy(
            int? BreadPointID = null,
            Guid? PublicID = null,
            string? Name = null,
            bool? IsActive = true)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", IsActive ?? (object)DBNull.Value);
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

        public static List<BreadPointModel.BreadPointInfoDTO> GetBreadPoints(
            bool? IsActive = true,
            int pageNumber = 1,int pageSize = 10)
        {
            var breadPointList = new List<BreadPointModel.BreadPointInfoDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IsActive", IsActive);
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
                    breadPointList.Add(MapToInfo(reader));
            }
            return breadPointList;
        }

        // ---------- Internal reads (return ObjDTO) ----------

        /// <summary>
        /// Internal-only lookup that joins BreadPoints with its owning User.
        /// Use for ownership/auth checks or any logic that needs both records together.
        /// Never return this DTO to the front end.
        /// </summary>
        public static BreadPointModel.BreadPointObjDTO? GetBreadPointObjBy(
            int? BreadPointID = null,
            Guid? PublicID = null,
            string? Name = null)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Get", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name" , Name?? (object) DBNull.Value);
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

        // ---------- Writes (take DataDTO, i.e. what the front end sends) ----------

        public static int CreateBreadPoint(BreadPointModel.BreadPointObjDTO breadPoint)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Create", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", breadPoint.UserID);
                cmd.Parameters.AddWithValue("@Name", breadPoint.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", breadPoint.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", breadPoint.PhoneNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AvailablePortions", breadPoint.AvailablePortions);
                cmd.Parameters.AddWithValue("@WalletBalance", breadPoint.WalletBalance);
                cmd.Parameters.AddWithValue("@Latitude", breadPoint.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", breadPoint.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", breadPoint.IsActive);

                var outputParam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
        }

        public static bool UpdateBreadPoint(BreadPointModel.BreadPointObjDTO breadPoint)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BreadPointID", breadPoint.BreadPointID < 1 ? (object)DBNull.Value : breadPoint.BreadPointID);
                cmd.Parameters.AddWithValue("@Name", breadPoint.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", breadPoint.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", breadPoint.PhoneNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AvailablePortions", breadPoint.AvailablePortions);
                cmd.Parameters.AddWithValue("@WalletBalance", breadPoint.WalletBalance);
                cmd.Parameters.AddWithValue("@Latitude", breadPoint.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", breadPoint.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", breadPoint.IsActive);

                var outputParam = new SqlParameter()
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputParam.Value > 0;
            }
        }

        public static bool DeleteBreadPoint(int BreadPointID, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_BreadPoints_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);
                cmd.Parameters.AddWithValue("@HardDelete", HardDelete);

                var outputParam = new SqlParameter()
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputParam.Value > 0;
            }
        }
    }
}