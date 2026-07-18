//using BreadApp_DL;

//namespace BreadApp_BL
//{
//    public class BreadPoints : Users
//    {
//        public enum enMode { Add = 1, Update = 2 }
//        enMode _Mode;


//        public int BreadPointID { get; set; } = -1;
//        public Guid BPPublicID { get; set; } = Guid.NewGuid();
//        public string Name { get; set; } = string.Empty;
//        public string Address { get; set; } = string.Empty;
//        public string PhoneNumber { get; set; } = string.Empty;
//        public int AvailablePortions { get; set; } = 0;
//        public decimal BPWalletBalance { get; set; } = 0;
//        public double? Latitude { get; set; } = 0d;
//        public double? Longitude { get; set; } = 0d;
//        public bool BPIsActive { get; set; } = true;
//        public DateTime BPCreatedAt { get; set; } = DateTime.Now;

//        public BreadPoints(): base()
//        { 
//            _Mode = enMode.Add;
//        }

//        public BreadPoints(BreadPointModel.BreadPointDataDTO BreadPointDTO , enMode Mode = enMode.Update)
//        {
//            this.BreadPointID = BreadPointDTO.BreadPointID;
//            this.PublicID = BreadPointDTO.PublicID;
//            this.Name = BreadPointDTO.Name;
//            this.Address = BreadPointDTO.Address;
//            this.PhoneNumber = BreadPointDTO.PhoneNumber;
//            this.AvailablePortions = BreadPointDTO.AvailablePortions;
//            this.WalletBalance = BreadPointDTO.WalletBalance;
//            this.Latitude = BreadPointDTO.Latitude;
//            this.Longitude = BreadPointDTO.Longitude;
//            this.IsActive = BreadPointDTO.IsActive;
//            this.CreatedAt = BreadPointDTO.CreatedAt;
//            _Mode = Mode;
//        }
//        public BreadPoints(BreadPointModel.BreadPointObjDTO BreadPointDTO, enMode Mode = enMode.Update)
//        {
//            this.BreadPointID = BreadPointDTO.BreadPointID;
//            this.PublicID = BreadPointDTO.PublicID;
//            this.Name = BreadPointDTO.Name;
//            this.Address = BreadPointDTO.Address;
//            this.PhoneNumber = BreadPointDTO.PhoneNumber;
//            this.AvailablePortions = BreadPointDTO.AvailablePortions;
//            this.WalletBalance = BreadPointDTO.WalletBalance;
//            this.Latitude = BreadPointDTO.Latitude;
//            this.Longitude = BreadPointDTO.Longitude;
//            this.IsActive = BreadPointDTO.IsActive;
//            this.CreatedAt = BreadPointDTO.CreatedAt;
//            _Mode = Mode;
//        }


//        public BreadPointModel.BreadPointInfoDTO ToInfoDTO()
//        {
//            return new BreadPointModel.BreadPointDTO(
//                this.BreadPointID,
//                this.PublicID,
//                this.Name,
//                this.Address,
//                this.PhoneNumber,
//                this.AvailablePortions,
//                this.WalletBalance,
//                this.Latitude,
//                this.Longitude,
//                this.IsActive,
//                this.CreatedAt
//            );
//        }
//        private BreadPointModel.BreadPointObjDTO _ToObjDTO()
//        {

//        }

//        private bool _AddBreadPoint()
//        {
//            this.BreadPointID = BreadPointsData.CreateBreadPoint(_ToDTO());
//            if (this.BreadPointID.HasValue)
//                return BreadPointID.Value > 0;
//            return false;
//        }

//        private bool _UpdateBreadPoint()
//        {
//            return BreadPointsData.UpdateBreadPoint(_ToDTO());
//        }

//        public bool Save()
//        {
//            switch (_Mode)
//            {
//                case enMode.Add:
//                    if (_AddBreadPoint())
//                    {
//                        _Mode = enMode.Update;
//                        return true;
//                    }
//                    else
//                        return false;
//                case enMode.Update:
//                    return _UpdateBreadPoint();
//                default:
//                    return false;
//            }

//        }

//        public bool Delete(bool HardDelete = false)
//        {
//            if (this.BreadPointID.HasValue)
//                return BreadPointsData.DeleteBreadPoint(this.BreadPointID.Value, HardDelete);

//            return false;
//        }


//        public static List<BreadPointModel.BreadPointDTO> GetAllBreadPoints(
//            bool? IsActive = null, int PageNumber = 1, int PageSize = 10)
//        {
//            return BreadPointsData.GetBreadPoints(IsActive, PageNumber, PageSize);
//        }

//        public static BreadPointModel.BreadPointDTO? GetBreadPointBy(
//            int? BreadPointID = null, Guid? PublicID = null,
//            string? Name = null, bool? IsActive = null)
//        {
//            return BreadPointsData.GetBreadPointBy(BreadPointID, PublicID, Name, IsActive);
//        }

//        public static BreadPoints? Find(int BreadPointID)
//        {
//            var dto = BreadPointsData.GetBreadPointBy(BreadPointID: BreadPointID);
//            if (dto == null) return null;

//            return new BreadPoints(dto);
//        }
//    }
//}

using BreadApp_DL;

namespace BreadApp_BL
{
    public class BreadPoints : Users
    {
        public enum enMode { Add = 1, Update = 2 }
        enMode _Mode;

        // ---- BreadPoint's own data (kept separate from the inherited User's data) ----
        public int BreadPointID { get; set; } = -1;
        public Guid BPPublicID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int AvailablePortions { get; set; } = 0;
        public decimal BPWalletBalance { get; set; } = 0;
        public double? Latitude { get; set; } = 0d;
        public double? Longitude { get; set; } = 0d;
        public bool BPIsActive { get; set; } = true;
        public DateTime BPCreatedAt { get; set; } = DateTime.Now;

        public BreadPoints() : base()
        {
            _Mode = enMode.Add;
        }

        /// <summary>
        /// Built from what the front end sent (create/update payload).
        /// Only BreadPoint fields are populated here — this DTO does not carry the
        /// owning user's full profile, so the inherited Users fields are left untouched
        /// besides UserID, which is needed to know who owns this BreadPoint.
        /// </summary>
        public BreadPoints(BreadPointModel.BreadPointDataDTO BreadPointDTO, enMode Mode = enMode.Update)
        {
            this.BreadPointID = BreadPointDTO.BreadPointID;
            this.BPPublicID = BreadPointDTO.PublicID;
            this.UserID = BreadPointDTO.UserID;
            this.Name = BreadPointDTO.Name;
            this.Address = BreadPointDTO.Address;
            this.PhoneNumber = BreadPointDTO.PhoneNumber;
            this.AvailablePortions = BreadPointDTO.AvailablePortions?? this.AvailablePortions;
            this.BPWalletBalance = BreadPointDTO.WalletBalance?? this.BPWalletBalance;
            this.Latitude = BreadPointDTO.Latitude;
            this.Longitude = BreadPointDTO.Longitude;
            this.BPIsActive = BreadPointDTO.IsActive??this.BPIsActive;
            this.BPCreatedAt = BreadPointDTO.CreatedAt;
            _Mode = Mode;
        }

        /// <summary>
        /// Built from the full internal object (BreadPoint + owning User). Populates both
        /// the BreadPoint-specific fields and the inherited User fields, since ObjDTO carries both.
        /// </summary>
        public BreadPoints(BreadPointModel.BreadPointObjDTO BreadPointDTO, enMode Mode = enMode.Update)
            : base( BreadPointDTO)
        {
            this.BreadPointID = BreadPointDTO.BreadPointID;
            this.BPPublicID = BreadPointDTO.BPPublicID;
            this.Name = BreadPointDTO.Name;
            this.Address = BreadPointDTO.Address;
            this.PhoneNumber = BreadPointDTO.PhoneNumber;
            this.AvailablePortions = BreadPointDTO.AvailablePortions;
            this.BPWalletBalance = BreadPointDTO.BPWalletBalance;
            this.Latitude = BreadPointDTO.Latitude;
            this.Longitude = BreadPointDTO.Longitude;
            this.BPIsActive = BreadPointDTO.BPIsActive;
            this.BPCreatedAt = BreadPointDTO.BPCreatedAt;
            _Mode = Mode;
        }

        /// <summary>
        /// What we hand back to the front end. No user profile data included.
        /// </summary>
        public BreadPointModel.BreadPointInfoDTO ToInfoDTO()
        {
            return new BreadPointModel.BreadPointInfoDTO(
                this.BreadPointID,
                this.BPPublicID,
                this.UserID,
                this.Name,
                this.Address,
                this.PhoneNumber,
                this.AvailablePortions,
                this.BPWalletBalance,
                this.Latitude,
                this.Longitude,
                this.BPIsActive,
                this.BPCreatedAt
            );
        }

        /// <summary>
        /// Full internal object, used to pass this BreadPoint (with its owning user) around
        /// inside the program. Never returned from a controller/endpoint.
        /// </summary>
        private BreadPointModel.BreadPointObjDTO _ToObjDTO()
        {
            return new BreadPointModel.BreadPointObjDTO(
                breadPointID: this.BreadPointID,
                BPpublicID: this.BPPublicID,
                userID: this.UserID,
                name: this.Name,
                address: this.Address,
                phoneNumber: this.PhoneNumber,
                availablePortions: this.AvailablePortions,
                BPwalletBalance: this.BPWalletBalance,
                latitude: this.Latitude,
                longitude: this.Longitude,
                BPisActive: this.BPIsActive,
                BPcreatedAt: this.BPCreatedAt,
                UserID: this.UserID,
                PublicID: this.PublicID,
                FirstName: this.FirstName,
                SecondName: this.SecondName,
                LastName: this.LastName,
                DateOfBirth: this.DateOfBirth,
                MaritalStatus: this.MaritalStatus,
                FamilyNumber: this.FamilyNumber,
                Phone: this.Phone,
                WalletBalance: this.WalletBalance,
                WifeNational: this.WifeNational,
                HusbNational: this.HusbNational,
                IsActive: this.IsActive,
                CreatedAt: this.CreatedAt,
                NationalNumber: this.NationalNumber,
                PasswordHash: this.PasswordHash,
                Role: this.Role,
                RefreshTokenHash: this.RefreshTokenHash,
                RefreshTokenExpiresAt: this.RefreshTokenExpiresAt,
                RefreshTokenRevokedAt: this.RefreshTokenRevokedAt
            );
        }

        private bool _AddBreadPoint()
        {
            //this.UserID = UsersData.CreateUser(_ToObjDTO());
            this.BreadPointID = BreadPointsData.CreateBreadPoint(_ToObjDTO());
            return this.BreadPointID > 0;
        }

        private bool _UpdateBreadPoint()
        {
            return BreadPointsData.UpdateBreadPoint(_ToObjDTO());
        }

        public bool Save()
        {
            base.Save(); // Save the inherited User fields first, then handle the BreadPoint-specific fields.

            switch (_Mode)
            {
                case enMode.Add:
                   
                    if (_AddBreadPoint())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateBreadPoint();
                default:
                    return false;
            }
        }

        public bool Delete(bool HardDelete = false)
        {
            if (this.BreadPointID > 0)
                return BreadPointsData.DeleteBreadPoint(this.BreadPointID, HardDelete);

            return false;
        }

        public static bool Delete( int BreadPointID , bool HardDelete = false)
        {
            if (BreadPointID > 0)
                return BreadPointsData.DeleteBreadPoint(BreadPointID, HardDelete);

            return false;
        }
        /// <summary>
        /// Front-facing list — returns InfoDTOs, safe to hand straight to a controller/API response.
        /// </summary>
        public static List<BreadPointModel.BreadPointInfoDTO> GetAllBreadPoints(
            bool? IsActive = true, int PageNumber = 1, int PageSize = 10)
        {
            return BreadPointsData.GetBreadPoints(IsActive, PageNumber, PageSize);
        }

        /// <summary>
        /// Front-facing single lookup — returns an InfoDTO, safe to hand straight to a controller/API response.
        /// </summary>
        public static BreadPointModel.BreadPointInfoDTO? GetBreadPointBy(
            int? BreadPointID = null, Guid? PublicID = null,
            string? Name = null, bool? IsActive = null)
        {
            return BreadPointsData.GetBreadPointBy(BreadPointID:BreadPointID, PublicID:PublicID, Name:Name, IsActive);
        }

        /// <summary>
        /// Internal-only loader — pulls the full ObjDTO (BreadPoint + owning User) so the
        /// returned instance is ready to Save()/Delete() or be inspected for ownership checks.
        /// </summary>
        public static BreadPoints? Find(int BreadPointID)
        {
            var dto = BreadPointsData.GetBreadPointObjBy(BreadPointID: BreadPointID);
            if (dto == null)
                return null;

            return new BreadPoints(dto);
        }
    }
}


