using BreadApp_Struct.Common;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Data;
using static BreadApp_DL.UserModel;
using static BreadApp_Struct.Models.UserModel;

namespace BreadApp_DL
{
    public class UserModel
    {
        // this is what i get from the Front
        public class UserDataDTO
        {

            public string? NationalNumber { get; set; } = null;
            public string? FirstName { get; set; } = null;
            public string? SecondName { get; set; } = null;
            public string? LastName { get; set; } = null;
            public DateTime? DateOfBirth { get; set; } = null;
            public bool? MaritalStatus { get; set; } = null;
            public int? FamilyNumber { get; set; } = null;
            public string? Phone { get; set; } = null;
            public string? Password { get; set; } = null;
            public string? WifeNational { get; set; } = null;
            public string? HusbNational { get; set; } = null;
        }
        // this is what i gave for the front
        public class UserInfoDTO
        {
            public UserInfoDTO(int UserID, Guid PublicID, string NationalNumber,
                string FirstName, string SecondName, string LastName, DateTime DateOfBirth,
                bool MaritalStatus, int? FamilyNumber, string Phone, decimal WalletBalance,
                string? WifeNational , string? HusbNational, bool IsActive, DateTime CreatedAt , string Role)
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
                this.IsActive = IsActive;
                this.CreatedAt = CreatedAt;
                this.Role = Role;
                this.WifeNational = WifeNational;
                this.HusbNational = HusbNational;
            }
            public int UserID { get; set; }
            public Guid PublicID { get; set; }
            public string NationalNumber { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public bool MaritalStatus { get; set; }
            public int? FamilyNumber { get; set; }
            public string Phone { get; set; }
            public decimal WalletBalance { get; set; }
            public string? WifeNational { get; set; }
            public string? HusbNational { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Role {  get; set; }

        }
        // this is what i Transfer inside my system
        public class UserObjDTO
        {
            public UserObjDTO(int UserID , Guid PublicID , string FirstName , string SecondName,
                string LastName , DateTime DateOfBirth , bool MaritalStatus , int? FamilyNumber ,
                string Phone , decimal WalletBalance , string? WifeNational , string? HusbNational,
                bool IsActive , DateTime CreatedAt , string NationalNumber , string PasswordHash,
                string Role , string? RefreshTokenHash, DateTime? RefreshTokenExpiresAt, 
                DateTime? RefreshTokenRevokedAt)
            {
                this.UserID = UserID;
                this.PublicID = PublicID;
                this.FirstName = FirstName;
                this.SecondName= SecondName;
                this.LastName = LastName;
                this.DateOfBirth = DateOfBirth;
                this.MaritalStatus = MaritalStatus;
                this.FamilyNumber = FamilyNumber;
                this.Phone = Phone;
                this.WalletBalance = WalletBalance;
                this.WifeNational = WifeNational;
                this.HusbNational = HusbNational;
                this.IsActive = IsActive;
                this.CreatedAt = CreatedAt;
                this.NationalNumber = NationalNumber;
                this.PasswordHash = PasswordHash;
                this.Role = Role;
                this.RefreshTokenHash = RefreshTokenHash;
                this.RefreshTokenExpiresAt = RefreshTokenExpiresAt;
                this.RefreshTokenRevokedAt = RefreshTokenRevokedAt;
            }
            public int UserID { get; set; }
            public Guid PublicID { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public bool MaritalStatus { get; set; }
            public int? FamilyNumber { get; set; }
            public string Phone { get; set; }
            public decimal WalletBalance { get; set; }
            public string? WifeNational { get; set; }
            public string? HusbNational { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }

            // Authentication-related fields
            public string NationalNumber { get; set; }
            public string PasswordHash { get; set; }
            public string Role { get; set; }

            public string? RefreshTokenHash { get; set; }
            public DateTime? RefreshTokenExpiresAt { get; set; }
            public DateTime? RefreshTokenRevokedAt { get; set; }

        }
    }
    public class UsersData
    {


        private static UserObjDTO MapRow(SqlDataReader reader)
        {
            
            return new UserObjDTO(
                UserID: reader.GetInt32(reader.GetOrdinal("UserID")),
                PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                NationalNumber: reader.GetString(reader.GetOrdinal("NationalNumber")),
                FirstName:  reader.GetString(reader.GetOrdinal("FirstName")),
                SecondName:reader.GetString(reader.GetOrdinal("SecondName")),
                LastName: reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth: reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                MaritalStatus:reader.GetBoolean(reader.GetOrdinal("MaritalStatus")),
                FamilyNumber: reader.IsDBNull(reader.GetOrdinal("FamilyNumber")) ? null : reader.GetInt32(reader.GetOrdinal("FamilyNumber")),
                Phone: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                WalletBalance: reader.GetDecimal(reader.GetOrdinal("WalletBalance")),
                WifeNational: reader.IsDBNull(reader.GetOrdinal("WifeNationalNum")) ? null : reader.GetString(reader.GetOrdinal("WifeNationalNum")),
                HusbNational: reader.IsDBNull(reader.GetOrdinal("HusbNationalNum")) ? null : reader.GetString(reader.GetOrdinal("HusbNationalNum")),
                IsActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt:reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                Role: reader.GetString(reader.GetOrdinal("Role")),
                PasswordHash: reader.GetString(reader.GetOrdinal("PasswordHash")),
                RefreshTokenHash: reader.IsDBNull(reader.GetOrdinal("RefreshTokenHash")) ? null : reader.GetString(reader.GetOrdinal("RefreshTokenHash")),
                RefreshTokenExpiresAt: reader.IsDBNull(reader.GetOrdinal("RefreshTokenExpiresAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt")),
                RefreshTokenRevokedAt: reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt"))
            );
            
                
            
        }
        private static UserInfoDTO MapRowEndUser(SqlDataReader reader)
        {
            return new UserInfoDTO(
                    UserID: reader.GetInt32(reader.GetOrdinal("UserID")),
                    PublicID: reader.GetGuid(reader.GetOrdinal("PublicID")),
                    NationalNumber: reader.GetString(reader.GetOrdinal("NationalNumber")),
                    FirstName: reader.GetString(reader.GetOrdinal("FirstName")),
                    SecondName: reader.GetString(reader.GetOrdinal("SecondName")),
                    LastName: reader.GetString(reader.GetOrdinal("LastName")),
                    DateOfBirth: reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    MaritalStatus: reader.GetBoolean(reader.GetOrdinal("MaritalStatus")),
                    FamilyNumber: reader.IsDBNull(reader.GetOrdinal("FamilyNumber")) ? null : reader.GetInt32(reader.GetOrdinal("FamilyNumber")),
                    Phone: reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    WalletBalance: reader.GetDecimal(reader.GetOrdinal("WalletBalance")),
                    WifeNational: reader.IsDBNull(reader.GetOrdinal("WifeNationalNum")) ? null : reader.GetString(reader.GetOrdinal("WifeNationalNum")),
                    HusbNational: reader.IsDBNull(reader.GetOrdinal("HusbNationalNum")) ? null : reader.GetString(reader.GetOrdinal("HusbNationalNum")),
                    IsActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    Role: reader.GetString(reader.GetOrdinal("Role"))
                );
        }
        private static UserByBreadPointDTO MapRowUserByBreadPoint(SqlDataReader reader)
        {
            return new UserByBreadPointDTO(
                    UserID: reader.GetInt32(reader.GetOrdinal("UserID")),
                    NationalNumber: reader.GetString(reader.GetOrdinal("NationalNumber")),
                    FullName: reader.GetString(reader.GetOrdinal("FullName")),
                    CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    IsScanned: reader.GetBoolean(reader.GetOrdinal("IsScanned")),
                    ScannedAt: reader.IsDBNull(reader.GetOrdinal("ScannedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ScannedAt"))
                );
        }




        public static UserObjDTO? GetUserBy(int? UserID  = null , Guid? PublicID = null ,
            String? NationalNumber = null, string? Phone = null)
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

                    var outputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);



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

        public static List<UserModel.UserObjDTO> GetUsers(bool? IsActive = true, int pageNumber = 1, int pageSize = 10)
        {
           
            var UserList = new List<UserObjDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber< 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize<1 ? 10 : pageSize);
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

        public static List<UserByBreadPointDTO> GetUsers(int? BreadPointID, int pageNumber = 1, int pageSize = 10)
        {

            var UserList = new List<UserByBreadPointDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);
                    var OutputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OutputParam);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserList.Add(MapRowUserByBreadPoint(reader));
                    }
                }

            }

            return UserList;
        }

        public static List<UserModel.UserInfoDTO> GetUsers(bool? IsActive = true, int pageNumber = 1, int pageSize = 10, bool endUser = false)
        {

            var UserList = new List<UserInfoDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive ?? (object)DBNull.Value);
                    var OutputParam = new SqlParameter("@RecordCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OutputParam);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserList.Add(MapRowEndUser(reader));
                    }
                }

            }

            return UserList;
        }
      
        
        
        
        public static int CreateUser(UserModel.UserObjDTO user , SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NationalNumber", user.NationalNumber);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", user.SecondName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@MaritalStatus", user.MaritalStatus);
                    cmd.Parameters.AddWithValue("@FamilyNumber", user.FamilyNumber < 1 ? (object)DBNull.Value : user.FamilyNumber);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.Phone);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash );
                    cmd.Parameters.AddWithValue("@WifeNationalNum", user.WifeNational??(object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HusbNationalNum", user.HusbNational ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    var outputParam = new SqlParameter("@NewUserID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);  
                    conn.Open();
                    SetSessionContext(conn, sessionInfo);

                    cmd.ExecuteNonQuery();
                    return (int)outputParam.Value;
                }
            }
          
        }

        public static bool UpdateUser(UserModel.UserObjDTO user , SessionContextInfo sessionInfo)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", user.SecondName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@MaritalStatus", user.MaritalStatus);
                    cmd.Parameters.AddWithValue("@FamilyNumber", user.FamilyNumber);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.Phone);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@WifeNationalNum", user.WifeNational ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HusbNationalNum", user.HusbNational ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    
                    cmd.Parameters.AddWithValue("@RefreshTokenHash", user.RefreshTokenHash);
                    cmd.Parameters.AddWithValue("@RefreshTokenExpiresAt", user.RefreshTokenExpiresAt);
                    cmd.Parameters.AddWithValue("@RefreshTokenRevokedAt", user.RefreshTokenRevokedAt);


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

        public static bool DeleteUser(int UserID, SessionContextInfo sessionInfo, bool HardDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);
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
