namespace BreadApp_BL
{
    public class Users
    {
        enum enMode { Add = 1 , Update =2 }
        enMode _Mode;
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

        public Users()
        {
        
        }

    }
}
