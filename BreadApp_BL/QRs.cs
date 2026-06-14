using BreadApp_DL;

namespace BreadApp_BL
{
    public class QRCodes
    {
        public enum enStatus { Active = 1, Scanned = 2 , Expired = 3, Cancelled = 4 }
        enum enMode { Add = 1, Update = 2 }
        enMode _Mode;

        public int? QRCodeID { get; set; }
        public Guid? PublicID { get; set; }
        public int? UserID { get; set; }
        public int? BreadPointID { get; set; }
        public int? PortionCount { get; set; }
        public enStatus? Status { get; set; }
        public string? Token { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime? ScannedAt { get; set; }
        public bool? IsScanned { get; set; }

        public QRCodes()
        {
            QRCodeID = null;
            PublicID = null;
            UserID = null;
            BreadPointID = null;
            PortionCount = 1;
            Status = enStatus.Active;
            Token = null;
            CreatedAt = null;
            ExpiresAt = null;
            ScannedAt = null;
            IsScanned = false;
            _Mode = enMode.Add;
        }

        private QRCodes(int QRCodeID, Guid PublicID, int UserID, int BreadPointID,
            int PortionCount, enStatus? Status, string Token,
            DateTime CreatedAt, DateTime ExpiresAt, DateTime? ScannedAt, bool IsScanned)
        {
            this.QRCodeID = QRCodeID;
            this.PublicID = PublicID;
            this.UserID = UserID;
            this.BreadPointID = BreadPointID;
            this.PortionCount = PortionCount;
            this.Status = Status;
            this.Token = Token;
            this.CreatedAt = CreatedAt;
            this.ExpiresAt = ExpiresAt;
            this.ScannedAt = ScannedAt;
            this.IsScanned = IsScanned;
            _Mode = enMode.Update;
        }

        private QRCodeModel.QRCodeDTO _ToDTO()
        {
            return new QRCodeModel.QRCodeDTO(
                this.QRCodeID,
                this.PublicID,
                this.UserID,
                this.BreadPointID,
                this.PortionCount,
                (int)this.Status,
                this.Token,
                this.CreatedAt,
                this.ExpiresAt,
                this.ScannedAt,
                this.IsScanned
            );
        }

        private bool _AddQRCode()
        {
            if (!this.UserID.HasValue || !this.BreadPointID.HasValue)
                return false;

            this.BreadPointID = QRCodesData.CreateQRCode(
                this.UserID.Value,
                this.BreadPointID.Value,
                this.PortionCount ?? 1,
                this.ExpiresAt
            );

            if (this.BreadPointID.HasValue)
                return this.BreadPointID.Value > 0;
            return false;
        }

        private bool _UpdateQRCode()
        {
            return QRCodesData.UpdateQRCode(_ToDTO());
        }

        public bool Save()
        {
            if (_Mode == enMode.Add)
                if (_AddQRCode())
                {
                    _Mode = enMode.Add;
                    return true;
                }
                else if (_Mode == enMode.Update)
                    return _UpdateQRCode();

            return false;
        }

        public bool Delete(bool HardDelete = false)
        {
            if (this.QRCodeID.HasValue)
                return QRCodesData.DeleteQRCode(this.QRCodeID.Value, HardDelete);

            return false;
        }

      

        public static QRCodeModel.QRScanResultDTO? Scan(string Token, int BreadPointID)
        {
            return QRCodesData.ScanQRCode(Token, BreadPointID);
        }

        

        public static QRCodeModel.QRCodeDTO? GetQRCodeBy(
            int? QRCodeID = null, Guid? PublicID = null,
            int? UserID = null, int? BreadPointID = null,
            string? Token = null, string? Status = null,
            bool? IsScanned = null)
        {
            return QRCodesData.GetQRCodeBy(QRCodeID, PublicID, UserID, BreadPointID, Token, Status, IsScanned);
        }

        public static QRCodeModel.QRCodeDTO? GetQRCodeByID(int QRCodeID)
        {
            return QRCodesData.GetQRCodeBy(QRCodeID: QRCodeID);
        }

        public static QRCodeModel.QRCodeDTO? GetQRCodeByPublicID(Guid PublicID)
        {
            return QRCodesData.GetQRCodeBy(PublicID: PublicID);
        }

        public static QRCodeModel.QRCodeDTO? GetQRCodeByToken(string Token)
        {
            return QRCodesData.GetQRCodeBy(Token: Token);
        }

        public static List<QRCodeModel.QRCodeDTO> GetAllQRCodes(
            int? UserID = null, int? BreadPointID = null,
            string? Status = null, bool? IsScanned = null,
            int PageNumber = 1, int PageSize = 10)
        {
            return QRCodesData.GetQRCodes(UserID, BreadPointID, Status, IsScanned, PageNumber, PageSize);
        }

        public static List<QRCodeModel.QRCodeDTO> GetActiveQRCodesByUser(int UserID)
        {
            return QRCodesData.GetQRCodes(UserID: UserID, Status: "Active", IsScanned: false);
        }

        public static QRCodes? Find(int QRCodeID)
        {
            var dto = QRCodesData.GetQRCodeBy(QRCodeID: QRCodeID);
            if (dto == null) return null;
           
            return new QRCodes(
                dto.QRCodeID!.Value,
                dto.PublicID!.Value,
                dto.UserID!.Value,
                dto.BreadPointID!.Value,
                dto.PortionCount!.Value,
                (enStatus)dto.Status,
                dto.Token!,
                dto.CreatedAt!.Value,
                dto.ExpiresAt!.Value,
                dto.ScannedAt,
                dto.IsScanned!.Value
            );
        }
    }
}
