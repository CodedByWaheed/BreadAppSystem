using Microsoft.Data.SqlClient;
using System.Data;
using static BreadApp_DL.UserModel;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace BreadApp_DL
{
    public class UserModel
    {
        public class UserDTO
        {
          public UserDTO(int? UserID , Guid? PublicID , string? NationalNumber, string? FirstName
                , string? SecondName, string? LastName, DateTime? DateOfBirth, bool? MaritalStatus
                , int? FamilyNumber, string? Phone, string? PasswordHash, Decimal? WalletBalance,
                string? WifeHusbNational, bool? IsActive, DateTime? CreatedAt , string? Role ,
                string? RefreshTokenHash , DateTime? RefreshTokenExpiresAt , DateTime? RefreshTokenRevokedAt)
            {
                this.UserID = UserID;
                this.PublicID = PublicID;
                this.NationalNumber = NationalNumber;
                this.FirstName = FirstName;
                this.SecondName = SecondName;
                this.LastName = LastName;
                this.DateOfBirth = DateOfBirth;
                this.MaritalStatus = MaritalStatus;
                this.FamilyNumber = FamilyNumber;
                this.Phone = Phone;
                this.PasswordHash = PasswordHash;
                this.WalletBalance = WalletBalance;
                this.WifeHusbNational = WifeHusbNational;
                this.IsActive = IsActive;
                this.CreatedAt = CreatedAt;
                this.Role = Role;
                this.RefreshTokenHash = RefreshTokenHash;
                this.RefreshTokenExpiresAt = RefreshTokenExpiresAt;
                this.RefreshTokenRevokedAt = RefreshTokenRevokedAt;
            }
            public int? UserID { get; set; }
            public Guid? PublicID { get; set; }
            public string? FirstName { get; set; }
            public string? SecondName { get; set; }
            public string? LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public bool? MaritalStatus { get; set; }
            public int? FamilyNumber { get; set; }
            public string? Phone { get; set; }
            public Decimal? WalletBalance { get; set; }
            public string? WifeHusbNational { get; set; }
            public bool? IsActive { get; set; }
            public DateTime? CreatedAt { get; set; }

            // Authentication-related fields
            public string? NationalNumber { get; set; }
            public string? PasswordHash { get; set; }
            public string? Role { get; set;  }

            public string? RefreshTokenHash { get; set; }
            public DateTime? RefreshTokenExpiresAt { get; set; }
            public DateTime? RefreshTokenRevokedAt { get; set; }

        }
        public class UserInfoDTO
        {
            public UserInfoDTO(int? UserID, Guid? PublicID, string? NationalNumber,
                string? FirstName, string? SecondName, string? LastName, DateTime? DateOfBirth,
                bool? MaritalStatus, int? FamilyNumber, string? Phone, Decimal? WalletBalance,
                string? WifeHusbNational, bool? IsActive, DateTime? CreatedAt , string? Role)
            {
                this.UserID = UserID;
                this.PublicID = PublicID;
                this.NationalNumber = NationalNumber;
                this.FirstName = FirstName;
                this.SecondName = SecondName;
                this.LastName = LastName;
                this.DateOfBirth = DateOfBirth;
                this.MaritalStatus = MaritalStatus;
                this.FamilyNumber = FamilyNumber;
                this.Phone = Phone;
                this.WalletBalance = WalletBalance;
                this.WifeHusbNational = WifeHusbNational;
                this.IsActive = IsActive;
                this.CreatedAt = CreatedAt;
                this.Role = Role;
            }
            public int? UserID { get; set; }
            public Guid? PublicID { get; set; }
            public string? NationalNumber { get; set; }
            public string? FirstName { get; set; }
            public string? SecondName { get; set; }
            public string? LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public bool? MaritalStatus { get; set; }
            public int? FamilyNumber { get; set; }
            public string? Phone { get; set; }
            public Decimal? WalletBalance { get; set; }
            public string? WifeHusbNational { get; set; }
            public bool? IsActive { get; set; }
            public DateTime? CreatedAt { get; set; }
            public string? Role {  get; set; }
        }
        public class LoginRequest
        {

            public string? NationalNo { get; set; }
            public string? Password { get; set; }
        }

   
    }
    public class UsersData
    {


        private static UserDTO MapRow(SqlDataReader reader)
        {
            return new UserDTO(
                UserID: reader.IsDBNull(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
                PublicID: reader.IsDBNull(reader.GetOrdinal("PublicID")) ? null : reader.GetGuid(reader.GetOrdinal("PublicID")),
                NationalNumber: reader.IsDBNull(reader.GetOrdinal("NationalNumber")) ? null : reader.GetString(reader.GetOrdinal("NationalNumber")),
                FirstName: reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                SecondName: reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName")),
                LastName: reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth: reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                MaritalStatus: reader.IsDBNull(reader.GetOrdinal("MaritalStatus")) ? null : reader.GetBoolean(reader.GetOrdinal("MaritalStatus")),
                FamilyNumber: reader.IsDBNull(reader.GetOrdinal("FamilyNumber")) ? null : reader.GetInt32(reader.GetOrdinal("FamilyNumber")),
                Phone: reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                WalletBalance: reader.IsDBNull(reader.GetOrdinal("WalletBalance")) ? null : reader.GetDecimal(reader.GetOrdinal("WalletBalance")),
                WifeHusbNational: reader.IsDBNull(reader.GetOrdinal("WifeHusb")) ? null : reader.GetString(reader.GetOrdinal("WifeHusb")),
                IsActive: reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                Role: reader.IsDBNull(reader.GetOrdinal("Role")) ? null : reader.GetString(reader.GetOrdinal("Role")),
                PasswordHash: reader.IsDBNull(reader.GetOrdinal("PasswordHash")) ? null : reader.GetString(reader.GetOrdinal("PasswordHash")),
                RefreshTokenHash: reader.IsDBNull(reader.GetOrdinal("RefreshTokenHash")) ? null : reader.GetString(reader.GetOrdinal("RefreshTokenHash")),
                RefreshTokenExpiresAt: reader.IsDBNull(reader.GetOrdinal("RefreshTokenExpiresAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt")),
                RefreshTokenRevokedAt: reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt"))
                );
        }

        public static UserDTO? GetUserBy(
            int? UserID  = null , 
            Guid? PublicID = null ,
            String? NationalNumber = null, 
            string? Phone = null,
            bool? IsActive = null)
        {
          
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", UserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublicID", PublicID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NationalNumber", NationalNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PhoneNumber", Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive ?? (object)DBNull.Value);
                    

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return MapRow(reader);
                    
                    }
                }
            }
            return null;
        }

        public static List<UserModel.UserDTO> GetUsers(bool? IsActive = true, int pageNumber = 1, int pageSize = 10)
        {
           
            var UserList = new List<UserDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting. ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@PageNumber", pageNumber< 1 ? 1 : pageNumber);
                    //cmd.Parameters.AddWithValue("@PageRow", pageSize<1 ? 10 : pageSize);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive ?? (object)DBNull.Value);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserList.Add(MapRow(reader));
                    }
                }
               
            }

            return UserList;
        }

        public static int CreateUser(UserModel.UserDTO user)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NationalNumber", user.NationalNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SecondName", user.SecondName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@MaritalStatus", user.MaritalStatus);
                    cmd.Parameters.AddWithValue("@FamilyNumber", user.FamilyNumber < 1 ? (object)DBNull.Value : user.FamilyNumber);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.Phone);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash );
                    cmd.Parameters.AddWithValue("@WifeHusb", user.WifeHusbNational );
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    var outputParam = new SqlParameter("@NewUserID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);  
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return (int)outputParam.Value;
                }
            }
          
        }

        public static bool UpdateUser(UserModel.UserDTO user)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", user.UserID < 1 ? (object)DBNull.Value : user.UserID);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SecondName", user.SecondName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@MaritalStatus", user.MaritalStatus);
                    cmd.Parameters.AddWithValue("@FamilyNumber", user.FamilyNumber < 1 ? (object)DBNull.Value : user.FamilyNumber);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.Phone);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@WifeHusb", user.WifeHusbNational??(Object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.Parameters.AddWithValue("@Role" , user.Role);
                    cmd.Parameters.AddWithValue("@RefreshTokenHash", user.RefreshTokenHash);
                    cmd.Parameters.AddWithValue("@RefreshTokenExpiresAt", user.RefreshTokenExpiresAt);
                    cmd.Parameters.AddWithValue("@RefreshTokenRevokedAt", user.RefreshTokenRevokedAt);


                    var outputParam = new SqlParameter("@OutputStatus", SqlDbType.Int)
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

        public static bool DeleteUser(int UserID, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@HardDelete", HardDelete);
                    var outputParam = new SqlParameter("@OutputStatus", SqlDbType.Int)
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

        public static UserModel.UserDTO? Authenticate(UserModel.LoginRequest request)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Authenticate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", request.NationalNo);
                    cmd.Parameters.AddWithValue("@Password", request.Password);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return MapRow(reader); 
                    }
                }
            }
            return null;
        }

    }
}
