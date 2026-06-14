using BreadApp_DL;

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
            this._Mode = Mode;
        }
        private Users(UserModel.UserInfoDTO UserDTO, enMode Mode = enMode.Update)
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
            this.WifeHusbNational = UserDTO.WifeHusbNational;
            this.IsActive = UserDTO.IsActive;
            this.CreatedAt = UserDTO.CreatedAt;
            this._Mode = Mode;
        }

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
                this.CreatedAt
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
                this.CreatedAt));
            
        }  
        public bool Save()
        {
            if (_Mode == enMode.Add)
                if (_AddUser())
                {
                    _Mode = enMode.Update;
                    return true;
                }
            else if (_Mode == enMode.Update)
                return _UpdateUser();

            return false;

        }
        public bool DeleteUser(bool HardDelete = false)
        {
            if(this.UserID.HasValue)
                return UsersData.DeleteUser(this.UserID.Value, HardDelete);

            return false;
        }
        public static List<UserModel.UserInfoDTO> GetAllUsers(int PageNumber = 1, int PageSize = 10)
        {
            return UsersData.GetUsers(PageNumber, PageSize);
        }
        public static UserModel.UserInfoDTO? GetUserBy(int? UserID, Guid? PublicID, String? NationalNumber, string? Phone, bool? IsActive)
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
        //public static UserModel.UserInfoDTO GetUserByID(int? UserID)
        //{
        //    return UsersData.GetUserBy(UserID, null, null, null, null);
        //}
        //public static UserModel.UserInfoDTO GetUserByPublicID( Guid? PublicID)
        //{
        //    return UsersData.GetUserBy(null, PublicID, null, null, null);
        //}
        //public static UserModel.UserInfoDTO GetUserByNationalNumber( string? NationalNumber)
        //{
        //    return UsersData.GetUserBy(null, null, NationalNumber, null, null);
        //}
        //public static UserModel.UserInfoDTO GetUserByPhone(string? Phone)
        //{
        //    return UsersData.GetUserBy(null, null, null, Phone, null);
        //}
        //public static UserModel.UserInfoDTO GetUserByActiveStatus(bool? IsActive)
        //{
        //    return UsersData.GetUserBy(null, null, null, null, IsActive);
        //}
        public static UserModel.UserInfoDTO Login(string? NationalNumber, string? PsswordHash)
        {
            return UsersData.Authenticate(new UserModel.LoginDTO(
                NationalNumber,
                PsswordHash
            ));
        }

        public static Users? Find(int UserID)
        {
            var dto = UsersData.GetUserBy(UserID:UserID);

           
            if (dto == null) return null;

            return new Users(dto);
        }
    }
}
