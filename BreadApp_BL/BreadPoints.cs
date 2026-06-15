using BreadApp_DL;

namespace BreadApp_BL
{
    public class BreadPoints
    {
        public enum enMode { Add = 1, Update = 2 }
        enMode _Mode;

        public int? BreadPointID { get; set; }
        public Guid? PublicID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int? AvailablePortions { get; set; }
        public Decimal? WalletBalance { get; set; }
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

        public BreadPoints(BreadPointModel.BreadPointDTO BreadPointDTO , enMode Mode = enMode.Update)
        {
            this.BreadPointID = BreadPointDTO.BreadPointID;
            this.PublicID = BreadPointDTO.PublicID;
            this.Name = BreadPointDTO.Name;
            this.Address = BreadPointDTO.Address;
            this.PhoneNumber = BreadPointDTO.PhoneNumber;
            this.AvailablePortions = BreadPointDTO.AvailablePortions;
            this.WalletBalance = BreadPointDTO.WalletBalance;
            this.Latitude = BreadPointDTO.Latitude;
            this.Longitude = BreadPointDTO.Longitude;
            this.IsActive = BreadPointDTO.IsActive;
            this.CreatedAt = BreadPointDTO.CreatedAt;
            _Mode = Mode;
        }

       
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
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddBreadPoint())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateBreadPoint();
                default:
                    return false;
            }
              
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

        public static BreadPoints? Find(int BreadPointID)
        {
            var dto = BreadPointsData.GetBreadPointBy(BreadPointID: BreadPointID);
            if (dto == null) return null;

            return new BreadPoints(dto);
        }
    }
}