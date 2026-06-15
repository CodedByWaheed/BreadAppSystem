using BreadApp_DL;

namespace BreadApp_BL
{
    public class QRs
    {
        public enum enStatus { Active = 1, Scanned = 2 , Expired = 3, Cancelled = 4 }
        public enum enMode { Add = 1, Update = 2 }
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

        public QRs()
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

        public QRs(QRCodeModel.QRCodeDTO QrDTO , enMode Mode = enMode.Update)
        {
            this.QRCodeID = QrDTO.QRCodeID;
            this.PublicID = QrDTO.PublicID;
            this.UserID = QrDTO.UserID;
            this.BreadPointID = QrDTO.BreadPointID;
            this.PortionCount = QrDTO.PortionCount;
            this.Status = (enStatus)QrDTO.Status;
            this.Token = QrDTO.Token;
            this.CreatedAt = QrDTO.CreatedAt;
            this.ExpiresAt = QrDTO.ExpiresAt;
            this.ScannedAt = QrDTO.ScannedAt;
            this.IsScanned = QrDTO.IsScanned;

            _Mode = Mode;
        }

        public QRs(QRCodeModel.CreateQRDTO cQrDTO, enMode Mode = enMode.Add)
        {

            this.QRCodeID = null;
            this.PublicID = null;
            this.UserID = cQrDTO.UserID;
            this.BreadPointID = cQrDTO.BreadPointID;
            this.PortionCount = cQrDTO.PortionCount;
            this.Status = enStatus.Active;
            this.Token = null;
            this.CreatedAt = DateTime.Now;
            this.ExpiresAt = DateTime.Now.AddHours(24);
            this.ScannedAt = null;
            this.IsScanned = null;

            _Mode = Mode;
       
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
        private QRCodeModel.CreateQRDTO _ToCreationDTO()
        {
            return new QRCodeModel.CreateQRDTO(
                
                this.UserID,
                this.BreadPointID,
                this.PortionCount,
                this.ExpiresAt
            );
        }

        private bool _AddQRCode()
        {
            if (!this.UserID.HasValue || !this.BreadPointID.HasValue)
                return false;

            this.BreadPointID = QRCodesData.CreateQRCode(_ToCreationDTO());
                

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
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddQRCode())
                    {
                        _Mode = enMode.Add;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateQRCode();
                    
                default:
                    return false;

            }
          
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

        

        public static QRCodeModel.QRCodeDTO? GetOneQRCodeBy(
            int? QRCodeID = null, Guid? PublicID = null, 
            string? Token = null)
        {
            return QRCodesData.GetOneQRCodeBy(QRCodeID:QRCodeID, PublicID:PublicID, Token:Token);
        }



        public static List<QRCodeModel.QRCodeDTO> GetAllQRCodes(
            int? UserID = null, int? BreadPointID = null,
            string? Status = null, bool? IsScanned = null,
            int PageNumber = 1, int PageSize = 10)
        {
            return QRCodesData.GetAllQRCodes(UserID: UserID, BreadPointID: BreadPointID, Status: Status, IsScanned: IsScanned, PageNumber: PageNumber, PageSize: PageSize);
        }

   
        public static QRs? Find(int QRCodeID)
        {
            var dto = QRCodesData.GetOneQRCodeBy(QRCodeID: QRCodeID);
            if (dto == null) return null;
           
            return new QRs(dto);
        }
    }
}
