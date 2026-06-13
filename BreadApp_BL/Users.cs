using BreadApp_DL;

namespace BreadApp_BL
{
    public class Users
    {
        enum enMode { Add = 1 , Update =2 }
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
        public double? WalletBalance { get; set; }
        public string? WifeHusbNational { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }

        public Users()
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
        }
        private Users(int UserID ,Guid PublicID , string NationalNumber , string FirstName , string SecondName , string LastName , 
            DateTime DateOfBirth , bool MaritalStatus , int FamilyNumber , string Phone , string PasswordHash
            , double WalletBalance , string WifeHusbNational , bool IsActive , DateTime CreatedAt)
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
        }

        private bool _AddUser()
        {
            int? UserID = UsersData.CreateUser(new UserModel.UserDTO
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
        public static UserModel.UserInfoDTO GetUserBy(int? UserID, Guid? PublicID, String? NationalNumber, string? Phone, bool? IsActive)
        {
            return UsersData.GetUserBy(UserID, PublicID, NationalNumber, Phone, IsActive);
        }
        public static UserModel.UserInfoDTO GetUserByID(int? UserID)
        {
            return UsersData.GetUserBy(UserID, null, null, null, null);
        }
        public static UserModel.UserInfoDTO GetUserByPublicID( Guid? PublicID)
        {
            return UsersData.GetUserBy(null, PublicID, null, null, null);
        }
        public static UserModel.UserInfoDTO GetUserByNationalNumber( String? NationalNumber)
        {
            return UsersData.GetUserBy(null, null, NationalNumber, null, null);
        }
        public static UserModel.UserInfoDTO GetUserByPhone(string? Phone)
        {
            return UsersData.GetUserBy(null, null, null, Phone, null);
        }
        public static UserModel.UserInfoDTO GetUserByActiveStatus(bool? IsActive)
        {
            return UsersData.GetUserBy(null, null, null, null, IsActive);
        }
        public static UserModel.UserInfoDTO Login(string? NationalNumber, string? PsswordHash)
        {
            return UsersData.Authenticate(new UserModel.LoginDTO(
                NationalNumber,
                PsswordHash
            ));
        }
}
}
