using BreadApp_DL;
using System.Net.Http.Headers;
using static BreadApp_DL.UserModel;

namespace BreadApp_BL
{
    public class Users
    {
        public enum enMode { Add = 1 , Update =2 }
        enMode _Mode;
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
        public string? PasswordHash { get; set; }
        public Decimal? WalletBalance { get; set; }
        public string? WifeHusbNational { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Role { get; set; }

        public string RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }



        public Users ()
        {
            UserID = null;
            PublicID = null;
            FirstName = null;
            SecondName = null;
            LastName = null;
            DateOfBirth = null;
            MaritalStatus = null;
            FamilyNumber = null;
            Phone = null;
            PasswordHash = null;
            WalletBalance = null;
            WifeHusbNational = null;
            IsActive = false;
            CreatedAt = null;
            _Mode = enMode.Add;
        }
        public Users(UserModel.UserDTO UserDTO ,enMode Mode = enMode.Update )
        {
            this.UserID = UserDTO.UserID;
            this.PublicID = UserDTO.PublicID;
            this.NationalNumber = UserDTO.NationalNumber;
            this.FirstName = UserDTO.FirstName;
            this.SecondName = UserDTO.SecondName;
            this.LastName = UserDTO.LastName;
            this.DateOfBirth = UserDTO.DateOfBirth;
            this.MaritalStatus = UserDTO.MaritalStatus;
            this.FamilyNumber = UserDTO.FamilyNumber;
            this.Phone = UserDTO.Phone;
            this.WalletBalance = UserDTO.WalletBalance;
            this.PasswordHash = UserDTO.PasswordHash;
            this.WifeHusbNational = UserDTO.WifeHusbNational;
            this.IsActive = UserDTO.IsActive;
            this.CreatedAt = UserDTO.CreatedAt;
            this.Role = UserDTO.Role;
            this.RefreshTokenHash = UserDTO.RefreshTokenHash;
            this.RefreshTokenExpiresAt = UserDTO.RefreshTokenExpiresAt;
            this.RefreshTokenRevokedAt = UserDTO.RefreshTokenRevokedAt;
            this._Mode = Mode;
        }
        public Users(UserModel.UserInfoDTO UserDTO, enMode Mode = enMode.Update)
        {
            this.UserID = UserDTO.UserID;
            this.PublicID = UserDTO.PublicID;
            this.NationalNumber = UserDTO.NationalNumber;
            this.FirstName = UserDTO.FirstName;
            this.SecondName = UserDTO.SecondName;
            this.LastName = UserDTO.LastName;
            this.DateOfBirth = UserDTO.DateOfBirth;
            this.MaritalStatus = UserDTO.MaritalStatus;
            this.FamilyNumber = UserDTO.FamilyNumber;
            this.Phone = UserDTO.Phone;
            this.WalletBalance = UserDTO.WalletBalance;
            this.PasswordHash = UserDTO.Password;
            this.WifeHusbNational = UserDTO.WifeHusbNational;
            this.IsActive = UserDTO.IsActive;
            this.CreatedAt = UserDTO.CreatedAt;
            this.Role = UserDTO.Role;
            this._Mode = Mode;
        }

        //private UserModel.UserDTO _ToDTO()
        //{
        //    return new UserDTO(
        //       this.UserID, PublicID,
        //       NationalNumber, FirstName, SecondName, LastName, DateOfBirth
        //       , MaritalStatus, FamilyNumber, Phone, PasswordHash, WalletBalance,
        //       WifeHusbNational, IsActive, CreatedAt, Role
        //    );
        //}

        private bool _AddUser()
        {
            this.UserID = UsersData.CreateUser(new UserModel.UserDTO
             (
                 this.UserID,
                 this.PublicID,
                 this.NationalNumber,
                 this.FirstName,
                 this.SecondName,
                 this.LastName,
                 this.DateOfBirth,
                 this.MaritalStatus,
                 this.FamilyNumber,
                 this.Phone,
                 this.PasswordHash,
                 this.WalletBalance,
                 this.WifeHusbNational,
                 this.IsActive,
                 this.CreatedAt,
                 this.Role,
                 this.RefreshTokenHash,
                 this.RefreshTokenExpiresAt,
                 this.RefreshTokenRevokedAt
             ));
            return UserID.Value > 0;
        }
        private bool _UpdateUser()
        {
            return UsersData.UpdateUser(new UserModel.UserDTO
            (
                this.UserID,
                this.PublicID,
                this.NationalNumber,
                this.FirstName,
                this.SecondName,
                this.LastName,
                this.DateOfBirth,
                this.MaritalStatus,
                this.FamilyNumber,
                this.Phone,
                this.PasswordHash,
                this.WalletBalance,
                this.WifeHusbNational,
                this.IsActive,
                this.CreatedAt,
                this.Role,
                this.RefreshTokenHash,
                this.RefreshTokenExpiresAt,
                this.RefreshTokenRevokedAt
                ));
            
        }  
        public bool Save()
        {
            switch (_Mode)
            {
                case  enMode.Add:
                    if(_AddUser())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else 
                        return false;
                    
                case enMode.Update:
                    return _UpdateUser();
                    
                default:
                    return false;

            }
           

        }
        public bool DeleteUser(bool HardDelete = false)
        {
            if(this.UserID.HasValue)
                return UsersData.DeleteUser(this.UserID.Value, HardDelete);

            return false;
        }
        public static bool DeleteUser(int? UserID ,bool HardDelete = false)
        {
            if (UserID.HasValue)
                return UsersData.DeleteUser(UserID.Value, HardDelete);

            return false;
        }
        public static List<UserModel.UserDTO> GetAllUsers(bool? IsActive = true , int PageNumber = 1, int PageSize = 10)
        {
            return UsersData.GetUsers(IsActive ,PageNumber, PageSize);
        }
        public static UserModel.UserDTO? GetUserBy(int? UserID = null, Guid? PublicID = null, String? NationalNumber = null, string? Phone = null, bool? IsActive = null)
        {
            if(UserID.HasValue)
                return UsersData.GetUserBy(UserID : UserID);
            if(PublicID.HasValue)
                return UsersData.GetUserBy(PublicID: PublicID);
            if (!string.IsNullOrEmpty(NationalNumber))
                return UsersData.GetUserBy(NationalNumber: NationalNumber);
            if (!string.IsNullOrEmpty(Phone))
                return UsersData.GetUserBy(Phone: Phone);
            if (IsActive.HasValue)
                return UsersData.GetUserBy(IsActive: IsActive);
            return null;

        }
        public static UserDTO? Authenticate(string? NationalNumber, string? Password)
        {
            UserModel.UserDTO User = Users.GetUserBy(NationalNumber: NationalNumber);
                                     //Users.GetAllUsers(PageNumber:1 , PageSize: 1).FirstOrDefault(Users => Users.NationalNumber == NationalNumber);
            if (User == null)
                return null;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(Password, User.PasswordHash);
            if(!isValidPassword)
                return null;

            return User;
        }

        public static Users? Find(int? UserID)
        {
            var dto = UsersData.GetUserBy(UserID:UserID);

           
            if (dto == null) return null;

            return new Users(dto);
        }
    }
}
