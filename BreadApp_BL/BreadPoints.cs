using BreadApp_DL;

namespace BreadApp_BL
{
    public class BreadPoints
    {
        enum enMode { Add = 1, Update = 2 }
        enMode _Mode;

        public int? BreadPointID { get; set; }
        public Guid? PublicID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int? AvailablePortions { get; set; }
        public double? WalletBalance { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }

        public BreadPoints()
        {
            BreadPointID = null;
            PublicID = null;
            Name = null;
            Address = null;
            PhoneNumber = null;
            AvailablePortions = null;
            WalletBalance = null;
            Latitude = null;
            Longitude = null;
            IsActive = false;
            CreatedAt = null;
            _Mode = enMode.Add;
        }

        private BreadPoints(int BreadPointID, Guid PublicID, string Name, string Address,
            string PhoneNumber, int AvailablePortions, double WalletBalance,
            double? Latitude, double? Longitude, bool IsActive, DateTime CreatedAt)
        {
            this.BreadPointID = BreadPointID;
            this.PublicID = PublicID;
            this.Name = Name;
            this.Address = Address;
            this.PhoneNumber = PhoneNumber;
            this.AvailablePortions = AvailablePortions;
            this.WalletBalance = WalletBalance;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.IsActive = IsActive;
            this.CreatedAt = CreatedAt;
            _Mode = enMode.Update;
        }

        // Convert the current instance to a DTO for data operations
        private BreadPointModel.BreadPointDTO _ToDTO()
        {
            return new BreadPointModel.BreadPointDTO(
                this.BreadPointID,
                this.PublicID,
                this.Name,
                this.Address,
                this.PhoneNumber,
                this.AvailablePortions,
                this.WalletBalance,
                this.Latitude,
                this.Longitude,
                this.IsActive,
                this.CreatedAt
            );
        }

        private bool _AddBreadPoint()
        {
            this.BreadPointID = BreadPointsData.CreateBreadPoint(_ToDTO());
            if (this.BreadPointID.HasValue)
                return BreadPointID.Value > 0;
            return false;
        }

        private bool _UpdateBreadPoint()
        {
            return BreadPointsData.UpdateBreadPoint(_ToDTO());
        }

        public bool Save()
        {
            if (_Mode == enMode.Add)
               if(_AddBreadPoint())
                {
                    _Mode = enMode.Update;
                    return true;
                }
               
            else if (_Mode == enMode.Update)
                return _UpdateBreadPoint();

            return false;
        }

        public bool Delete(bool HardDelete = false)
        {
            if (this.BreadPointID.HasValue)
                return BreadPointsData.DeleteBreadPoint(this.BreadPointID.Value, HardDelete);

            return false;
        }

     
        public static List<BreadPointModel.BreadPointDTO> GetAllBreadPoints(
            bool? IsActive = null, int PageNumber = 1, int PageSize = 10)
        {
            return BreadPointsData.GetBreadPoints(IsActive, PageNumber, PageSize);
        }

        public static BreadPointModel.BreadPointDTO? GetBreadPointBy(
            int? BreadPointID = null, Guid? PublicID = null,
            string? Name = null, bool? IsActive = null)
        {
            return BreadPointsData.GetBreadPointBy(BreadPointID, PublicID, Name, IsActive);
        }

        public static BreadPointModel.BreadPointDTO? GetBreadPointByID(int BreadPointID)
        {
            return BreadPointsData.GetBreadPointBy(BreadPointID: BreadPointID);
        }

        public static BreadPointModel.BreadPointDTO? GetBreadPointByPublicID(Guid PublicID)
        {
            return BreadPointsData.GetBreadPointBy(PublicID: PublicID);
        }

        public static BreadPointModel.BreadPointDTO? GetBreadPointByName(string Name)
        {
            return BreadPointsData.GetBreadPointBy(Name: Name);
        }

        public static List<BreadPointModel.BreadPointDTO> GetActiveBreadPoints(
            int PageNumber = 1, int PageSize = 10)
        {
            return BreadPointsData.GetBreadPoints(IsActive: true, pageNumber: PageNumber, pageSize: PageSize);
        }


        // This method retrieves a BreadPoint by its ID and converts it to a BreadPoints instance
        public static BreadPoints? Find(int BreadPointID)
        {
            var dto = BreadPointsData.GetBreadPointBy(BreadPointID: BreadPointID);
            if (dto == null) return null;

            return new BreadPoints(
                dto.BreadPointID!.Value,
                dto.PublicID!.Value,
                dto.Name!,
                dto.Address!,
                dto.PhoneNumber!,
                dto.AvailablePortions!.Value,
                dto.WalletBalance!.Value,
                dto.Latitude,
                dto.Longitude,
                dto.IsActive!.Value,
                dto.CreatedAt!.Value
            );
        }
    }
}