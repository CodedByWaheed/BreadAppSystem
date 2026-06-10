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
            public int UserID { get; set; }
            public int PublicID { get; set; }
            public string NationalNumber { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public bool MaritalStatus { get; set; }
            public int FamilyNumber { get; set; }
            public string Phone { get; set; }
            public string PasswordHash { get; set; }
            public double WalletBalance { get; set; }
            public string WifeHusbNational { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class CreateUserDTO
        {
            public string Username { get; set; }
            public string Password { get; set; } // hashed in service layer
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Role { get; set; }
        }

        public class UpdateUserDTO
        {
            public int UserID { get; set; }
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
        }

        public class LoginDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class PagedResult<T>
        {
            public List<T> Items { get; set; }
            public int TotalCount { get; set; }
        }
    }
    public class UsersData
    {

        private static string _connectionString ="Server=localhost;Database=BreadApp;User Id=sa;Password=123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";


        public static UserModel.UserDTO GetUserById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_GetById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new UserDTO
                        {
                        };
                    }
                }
            }
            return null;
        }

        public static UserModel.PagedResult<UserModel.UserDTO> GetUsers(int pageNumber, int pageSize)
        {
            List<UserDTO> users = new List<UserDTO>();
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_GetPaged", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(new UserDTO
                        {
                       
                        });
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        totalCount = (int)reader["TotalCount"];
                    }
                }
            }

            return new PagedResult<UserDTO>
            {
                Items = users,
                TotalCount = totalCount
            };
        }

        public static int CreateUser(UserModel.CreateUserDTO user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static bool UpdateUser(UserModel.UpdateUserDTO user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static UserDTO GetByUsername(string username)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_GetByUsername", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new UserDTO
                        {
                           
                        };
                    }
                }
            }
            return null;
        }



        public static UserModel.UserDTO Authenticate(UserModel.LoginDTO login)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_Authenticate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", login.Username);
                    cmd.Parameters.AddWithValue("@Password", login.Password);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new UserDTO
                        {
                           
                        };
                    }
                }
            }
            return null;
        }

    }
}
