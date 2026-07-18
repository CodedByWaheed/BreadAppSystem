using Microsoft.Data.SqlClient;
using System.Data;
using static BreadApp_DL.UserModel;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace BreadApp_DL
{
    public class UserModel
    {
        // this is what i get from the Front
        public class UserDataDTO
        {
          public UserDataDTO(string NationalNumber, string FirstName
                , string SecondName, string LastName, DateTime DateOfBirth, bool MaritalStatus
                , int? FamilyNumber, string Phone , string Password, string? WifeNational , string? HusbNational)
            {  
                this.NationalNumber = NationalNumber;
                this.FirstName = FirstName;
                this.SecondName = SecondName;
                this.LastName = LastName;
                this.DateOfBirth = DateOfBirth;
                this.MaritalStatus = MaritalStatus;
                this.FamilyNumber = FamilyNumber;
                this.Phone = Phone;
                this.Password = Password;
                this.WifeNational = WifeNational;
                this.HusbNational = HusbNational;
            }
            public string NationalNumber { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public bool MaritalStatus { get; set; }
            public int? FamilyNumber { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; } 
            public string? WifeNational { get; set; }
            public string? HusbNational { get; set; }
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
                WifeNational: reader.IsDBNull(reader.GetOrdinal("WifeNational")) ? null : reader.GetString(reader.GetOrdinal("WifeNational")),
                HusbNational: reader.IsDBNull(reader.GetOrdinal("HusbNational")) ? null : reader.GetString(reader.GetOrdinal("HusbNational")),
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
                    WifeNational: reader.IsDBNull(reader.GetOrdinal("WifeNational")) ? null : reader.GetString(reader.GetOrdinal("WifeNational")),
                    HusbNational: reader.IsDBNull(reader.GetOrdinal("HusbNational")) ? null : reader.GetString(reader.GetOrdinal("HusbNational")),
                    IsActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    Role: reader.GetString(reader.GetOrdinal("Role"))
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

        public static List<UserModel.UserObjDTO> GetUsers(int? BreadPointID , bool? IsActive = true, int pageNumber = 1, int pageSize = 10)
        {
           
            var UserList = new List<UserObjDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber< 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize<1 ? 10 : pageSize);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);
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

        public static List<UserModel.UserInfoDTO> GetUsers(int? BreadPointID, bool? IsActive = true, int pageNumber = 1, int pageSize = 10 ,bool EndUser = false)
        {

            var UserList = new List<UserInfoDTO>();

            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber < 1 ? 1 : pageNumber);
                    cmd.Parameters.AddWithValue("@PageRow", pageSize < 1 ? 10 : pageSize);
                    cmd.Parameters.AddWithValue("@BreadPointID", BreadPointID);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive ?? (object)DBNull.Value);

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
        public static int CreateUser(UserModel.UserObjDTO user)
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
                    cmd.Parameters.AddWithValue("@WifeNational", user.WifeNational??(object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HusbNational", user.HusbNational ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", true);
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

        public static bool UpdateUser(UserModel.UserObjDTO user)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", user.SecondName );
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@MaritalStatus", user.MaritalStatus);
                    cmd.Parameters.AddWithValue("@FamilyNumber", user.FamilyNumber);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.Phone);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@WifeNational", user.WifeNational ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HusbNational",user.HusbNational ?? (object)DBNull.Value);
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


    }
}
