using BreadApp_DL;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using static BreadApp_DL.UserModel;
using static BreadApp_Struct.Models.UserModel;

namespace BreadApp_BL
{
    public class Users
    {
        public enum enRole { User , BreadPoint , Admin }
        public enum enMode { Add = 1 , Update =2 }
        enMode _Mode;
        public int UserID { get; set; } = -1;
        public Guid PublicID { get; set; } = Guid.Empty;
        public string NationalNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string SecondName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public bool MaritalStatus { get; set; } = false;
        public int? FamilyNumber { get; set; } = null;
        public string Phone { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public decimal WalletBalance { get; set; } = 0;
        public string? WifeNational { get; set; } = string.Empty;
        public string? HusbNational { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Role { get; set; } = "User";
        public string? RefreshTokenHash { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiresAt { get; set; } = DateTime.Now;
        public DateTime? RefreshTokenRevokedAt { get; set; } = DateTime.Now;

        

        public Users ()
        {
            UserID = -1;
            PublicID = Guid.NewGuid() ;
            FirstName = "";
            SecondName = "";
            LastName = "";
            DateOfBirth = DateTime.MinValue;
            MaritalStatus = false;
            FamilyNumber = 0;
            Phone = "";
            PasswordHash = "";
            WalletBalance = 0;
            WifeNational = "";
            HusbNational = "";
            IsActive = false;
            CreatedAt = DateTime.MinValue;
            Role = "User";
            _Mode = enMode.Add;
        }
        public Users(UserDataDTO UserDTO, enMode Mode = enMode.Update)
        {
           
            this.NationalNumber = UserDTO.NationalNumber;
            this.FirstName = UserDTO.FirstName;
            this.SecondName = UserDTO.SecondName;
            this.LastName = UserDTO.LastName;
            this.DateOfBirth = UserDTO.DateOfBirth.HasValue ? UserDTO.DateOfBirth.Value : DateTime.MinValue ;
            this.MaritalStatus = UserDTO.MaritalStatus.HasValue ? UserDTO.MaritalStatus.Value : false;
            this.FamilyNumber = UserDTO.FamilyNumber;
            this.Phone = UserDTO.Phone;
            this.PasswordHash = UserDTO.Password;
            this.WifeNational = UserDTO.WifeNational;
            this.HusbNational = UserDTO.HusbNational;
            this._Mode = Mode;
        }

        public Users(UserObjDTO UserDTO, enMode Mode = enMode.Update)
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
            this.WifeNational = UserDTO.WifeNational;
            this.HusbNational= UserDTO.HusbNational;
            this.IsActive = UserDTO.IsActive;
            this.CreatedAt = UserDTO.CreatedAt;
            this.Role = UserDTO.Role;
            this.RefreshTokenHash = UserDTO.RefreshTokenHash;
            this.RefreshTokenExpiresAt = UserDTO.RefreshTokenExpiresAt;
            this.RefreshTokenRevokedAt = UserDTO.RefreshTokenRevokedAt;
            this._Mode = Mode;
        }

        private UserModel.UserObjDTO _ToObjDTO()
        {
            return new UserObjDTO(this.UserID, this.PublicID, this.FirstName,
                this.SecondName, this.LastName, this.DateOfBirth, this.MaritalStatus,
                this.FamilyNumber, this.Phone, this.WalletBalance, this.WifeNational,
                this.HusbNational, this.IsActive, this.CreatedAt, this.NationalNumber,
                this.PasswordHash, this.Role, this.RefreshTokenHash, this.RefreshTokenExpiresAt,
                this.RefreshTokenRevokedAt);
        }
        public UserInfoDTO ToInfoDTO()
        {
            return new UserInfoDTO(UserID: this.UserID, PublicID: this.PublicID , FirstName: this.FirstName,
               SecondName: this.SecondName, LastName:this.LastName, DateOfBirth:this.DateOfBirth,MaritalStatus:this.MaritalStatus,
               FamilyNumber: this.FamilyNumber, Phone:this.Phone, WalletBalance: this.WalletBalance,WifeNational: this.WifeNational,
               HusbNational: this.HusbNational,IsActive: this.IsActive,CreatedAt: this.CreatedAt,NationalNumber: this.NationalNumber,
               Role: this.Role);
        }

        private bool _AddUser(SessionContextInfo sessionInfo)
        {
            if (string.IsNullOrEmpty(NationalNumber))
                return false;
            this.UserID = UsersData.CreateUser(_ToObjDTO(), sessionInfo);
            return UserID > 0;
        }
        private bool _UpdateUser(SessionContextInfo sessionInfo)
        {
            return UsersData.UpdateUser(_ToObjDTO() , sessionInfo);  
        }  
        public bool Save(SessionContextInfo sessionInfo)
        {
            switch (_Mode)
            {
                case  enMode.Add:
                    if(_AddUser(sessionInfo))
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else 
                        return false;
                    
                case enMode.Update:
                    return _UpdateUser(sessionInfo);
                    
                default:
                    return false;

            }
           

        }

        public bool DeleteUser(SessionContextInfo sessionInfo ,bool HardDelete = false)
        {
            return UsersData.DeleteUser(this.UserID,sessionInfo, HardDelete);
        }
        public static bool DeleteUser(int UserID,SessionContextInfo sessionInfo , bool HardDelete = false)
        {
            return UsersData.DeleteUser(UserID,sessionInfo, HardDelete);
        }

        public static List<UserByBreadPointDTO> GetAllUsers(int?BreadPointID , int PageNumber = 1, int PageSize = 10)
        {
            return UsersData.GetUsers(BreadPointID, PageNumber, PageSize);
        }
        public static List<UserModel.UserInfoDTO> GetAllUsers( bool? IsActive = true, int PageNumber = 1, int PageSize = 10 )
        {
            return UsersData.GetUsers(IsActive, PageNumber, PageSize , endUser:true);
        }
        public static UserModel.UserObjDTO? GetUserBy(int? UserID = null, Guid? PublicID = null, String? NationalNumber = null, string? Phone = null)
        {
            if(UserID.HasValue)
                return UsersData.GetUserBy(UserID : UserID);
            if(PublicID.HasValue)
                return UsersData.GetUserBy(PublicID: PublicID);
            if (!string.IsNullOrEmpty(NationalNumber))
                return UsersData.GetUserBy(NationalNumber: NationalNumber);
            if (!string.IsNullOrEmpty(Phone))
                return UsersData.GetUserBy(Phone: Phone);
            
            return null;

        }
        public static UserObjDTO? Authenticate(string? NationalNumber, string? Password)
        {
            if (string.IsNullOrEmpty(NationalNumber) || string.IsNullOrEmpty(Password))
                return null;
            UserObjDTO User = Users.GetUserBy(NationalNumber: NationalNumber)!;
                                     
            if (User == null)
                return null;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(Password, User.PasswordHash);
            if(!isValidPassword)
                return null;

            return User;
        }

        public static Users? Find(int? UserID)
        {
            if (!UserID.HasValue)
                return null;
            UserObjDTO dto = UsersData.GetUserBy(UserID: UserID)!;

            if (dto == null) 
                return null;

            return new Users(dto);
        }
    }
}
