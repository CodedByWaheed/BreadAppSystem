//using BreadApp_DL;

//namespace BreadApp_BL
//{
//    public class QRs
//    {
//        public enum enStatus { Active = 1, Scanned = 2 , Expired = 3, Cancelled = 4 }
//        public enum enMode { Add = 1, Update = 2 }
//        enMode _Mode;

//        public int? QRCodeID { get; set; }
//        public Guid? PublicID { get; set; }
//        public int? UserID { get; set; }
//        public int? BreadPointID { get; set; }
//        public int? PortionCount { get; set; }
//        public enStatus? Status { get; set; }
//        public string? Token { get; set; }
//        public DateTime? CreatedAt { get; set; }
//        public DateTime? ExpiresAt { get; set; }
//        public DateTime? ScannedAt { get; set; }
//        public bool? IsScanned { get; set; }

//        public QRs()
//        {
//            QRCodeID = null;
//            PublicID = null;
//            UserID = null;
//            BreadPointID = null;
//            PortionCount = 1;
//            Status = enStatus.Active;
//            Token = null;
//            CreatedAt = null;
//            ExpiresAt = null;
//            ScannedAt = null;
//            IsScanned = false;
//            _Mode = enMode.Add;
//        }

//        public QRs(QRCodeModel.QRCodeDTO QrDTO , enMode Mode = enMode.Update)
//        {
//            this.QRCodeID = QrDTO.QRCodeID;
//            this.PublicID = QrDTO.PublicID;
//            this.UserID = QrDTO.UserID;
//            this.BreadPointID = QrDTO.BreadPointID;
//            this.PortionCount = QrDTO.PortionCount;
//            this.Status = (enStatus)QrDTO.Status;
//            this.Token = QrDTO.Token;
//            this.CreatedAt = QrDTO.CreatedAt;
//            this.ExpiresAt = QrDTO.ExpiresAt;
//            this.ScannedAt = QrDTO.ScannedAt;
//            this.IsScanned = QrDTO.IsScanned;

//            _Mode = Mode;
//        }

//        public QRs(QRCodeModel.CreateQRDTO cQrDTO, enMode Mode = enMode.Add)
//        {

//            this.QRCodeID = null;
//            this.PublicID = null;
//            this.UserID = cQrDTO.UserID;
//            this.BreadPointID = cQrDTO.BreadPointID;
//            this.PortionCount = cQrDTO.PortionCount;
//            this.Status = enStatus.Active;
//            this.Token = null;
//            this.CreatedAt = DateTime.Now;
//            this.ExpiresAt = DateTime.Now.AddHours(24);
//            this.ScannedAt = null;
//            this.IsScanned = null;

//            _Mode = Mode;

//        }
//        private QRCodeModel.QRCodeDTO _ToDTO()
//        {
//            return new QRCodeModel.QRCodeDTO(
//                this.QRCodeID,
//                this.PublicID,
//                this.UserID,
//                this.BreadPointID,
//                this.PortionCount,
//                this.Status.HasValue ? (int?)this.Status.Value : null,
//                this.Token,
//                this.CreatedAt,
//                this.ExpiresAt,
//                this.ScannedAt,
//                this.IsScanned
//            );
//        }
//        private QRCodeModel.CreateQRDTO _ToCreationDTO()
//        {
//            return new QRCodeModel.CreateQRDTO(

//                this.UserID,
//                this.BreadPointID,
//                this.PortionCount,
//                this.ExpiresAt
//            );
//        }

//        private bool _AddQRCode()
//        {
//            if (!this.UserID.HasValue || !this.BreadPointID.HasValue)
//                return false;

//            this.QRCodeID = QRCodesData.CreateQRCode(_ToCreationDTO());


//            if (this.BreadPointID.HasValue)
//                return this.BreadPointID.Value > 0;
//            return false;
//        }

//        private bool _UpdateQRCode()
//        {
//            return QRCodesData.UpdateQRCode(_ToDTO());
//        }

//        public bool Save()
//        {
//            switch (_Mode)
//            {
//                case enMode.Add:
//                    if (_AddQRCode())
//                    {
//                        _Mode = enMode.Update;
//                        return true;
//                    }
//                    return false;
//                case enMode.Update:
//                    return _UpdateQRCode();

//                default:
//                    return false;

//            }

//        }

//        public bool Delete(bool HardDelete = false)
//        {
//            if (this.QRCodeID.HasValue)
//                return QRCodesData.DeleteQRCode(this.QRCodeID.Value, HardDelete);

//            return false;
//        }



//        public static QRCodeModel.QRScanResultDTO? Scan(string Token, int BreadPointID)
//        {
//            return QRCodesData.ScanQRCode(Token, BreadPointID);
//        }



//        public static QRCodeModel.QRCodeDTO? GetOneQRCodeBy(
//            int? QRCodeID = null, Guid? PublicID = null, 
//            string? Token = null)
//        {
//            return QRCodesData.GetOneQRCodeBy(QRCodeID:QRCodeID, PublicID:PublicID, Token:Token);
//        }



//        public static List<QRCodeModel.QRCodeDTO> GetAllQRCodes(
//            int? UserID = null, int? BreadPointID = null,
//            string? Status = null, bool? IsScanned = null,
//            int PageNumber = 1, int PageSize = 10)
//        {
//            return QRCodesData.GetAllQRCodes(UserID: UserID, BreadPointID: BreadPointID, Status: Status, IsScanned: IsScanned, PageNumber: PageNumber, PageSize: PageSize);
//        }


//        public static QRs? Find(int QRCodeID)
//        {
//            var dto = QRCodesData.GetOneQRCodeBy(QRCodeID: QRCodeID);
//            if (dto == null) return null;

//            return new QRs(dto);
//        }
//    }
//}
using BreadApp_DL;

namespace BreadApp_BL
{
    public class QRs
    {
        public enum enStatus { Active = 1, Scanned = 2, Expired = 3, Cancelled = 4 }
        public enum enMode { Add = 1, Update = 2 }
        enMode _Mode;

        public int QRCodeID { get; set; } = -1;
        public Guid PublicID { get; set; } = Guid.Empty;
        public int UserID { get; set; } = -1;
        public int BreadPointID { get; set; } = -1;
        public int PortionCount { get; set; } = 0;
        public enStatus Status { get; set; } = enStatus.Active;
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; } = DateTime.MinValue;
        public DateTime? ScannedAt { get; set; } = null;
        public bool IsScanned { get; set; } = false;

        public QRs()
        {
            _Mode = enMode.Add;
        }

        /// <summary>
        /// Built from what the front end sent (create-a-QR request). Server-side fields
        /// (status/token/timestamps) get sane defaults here; the DB fills in Token/PublicID
        /// on save.
        /// </summary>
        public QRs(QRModel.QrDataDTO qrDTO, enMode Mode = enMode.Add)
        {
            
            this.UserID = qrDTO.UserID;
            this.BreadPointID = qrDTO.BreadPointID;
            this.PortionCount = qrDTO.PortionCount < 1 ? 1 : qrDTO.PortionCount;
            this.Status = enStatus.Active;
            this.Token = string.Empty;
            this.CreatedAt = DateTime.Now;
            this.ScannedAt = null;
            this.IsScanned = false;

            _Mode = Mode;
        }

        /// <summary>
        /// Built from the full internal object (used by Find()/Scan()), ready for further
        /// Save()/Delete() calls.
        /// </summary>
        public QRs(QRModel.QrObjDTO qrDTO, enMode Mode = enMode.Update)
        {
            this.QRCodeID = qrDTO.QRCodeID;
            this.PublicID = qrDTO.PublicID;
            this.UserID = qrDTO.UserID;
            this.BreadPointID = qrDTO.BreadPointID;
            this.PortionCount = qrDTO.PortionCount;
            this.Status = (enStatus)qrDTO.Status;
            this.Token = qrDTO.Token;
            this.CreatedAt = qrDTO.CreatedAt;
            this.ExpiresAt = qrDTO.ExpiresAt;
            this.ScannedAt = qrDTO.ScannedAt;
            this.IsScanned = qrDTO.IsScanned;

            _Mode = Mode;
        }

        /// <summary>
        /// What we hand back to the front end. Only call this once the QR is fully persisted
        /// (all the "required" fields below are guaranteed non-null at that point).
        /// </summary>
        public QRModel.QrInfoDTO ToInfoDTO()
        {
            return new QRModel.QrInfoDTO(
                this.QRCodeID,
                this.PublicID,
                this.UserID,
                this.BreadPointID,
                this.PortionCount,
                (int)this.Status,
                this.Token ,
                this.CreatedAt ,
                this.ExpiresAt ,
                this.ScannedAt ?? DateTime.MinValue,
                this.IsScanned
            );
        }

        /// <summary>
        /// What we send to the DL layer for the create-only path.
        /// </summary>
        private QRModel.QrDataDTO _ToDataDTO()
        {
            return new QRModel.QrDataDTO(
                this.UserID ,
                this.BreadPointID ,
                this.PortionCount 
            );
        }

        /// <summary>
        /// Full internal object, used for partial updates — only the fields that matter for
        /// an update are populated here; the rest stay null so the DL layer leaves them alone.
        /// </summary>
        private QRModel.QrObjDTO _ToObjDTO()
        {
            var obj = new QRModel.QrObjDTO(
                QRCodeID: this.QRCodeID,
                PublicID: this.PublicID,
                UserID: this.UserID,
                BreadPointID: this.BreadPointID,
                PortionCount: this.PortionCount,
                Status: (int)this.Status,
                ScannedAt: this.ScannedAt
            );

            obj.Token = this.Token;
            obj.CreatedAt = this.CreatedAt;
            obj.ExpiresAt = this.ExpiresAt;
            obj.IsScanned = this.IsScanned;

            return obj;
        }

        private bool _AddQRCode()
        {

            this.Token = QRCodesData.CreateQRCode(_ToDataDTO());

            return !string.IsNullOrEmpty( this.Token );
        }

        private bool _UpdateQRCode()
        {
            return QRCodesData.UpdateQRCode(_ToObjDTO());
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddQRCode())
                    {
                        _Mode = enMode.Update;
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
            return QRCodesData.DeleteQRCode(this.QRCodeID, HardDelete);
        }
        public static bool Delete(int QrID , bool HardDelete = false)
        {
            return QRCodesData.DeleteQRCode(QrID , HardDelete);
        }
        /// <summary>
        /// Scans a QR code at a bread point. Returns the internal BL object (not an InfoDTO)
        /// so the caller can inspect the result and decide what to expose to the front end,
        /// e.g. via ToInfoDTO() after any business-rule adjustments.
        /// </summary>
        public static QRs? Scan(string Token, int BreadPointID)
        {
            var dto = QRCodesData.ScanQRCode(Token, BreadPointID);
            if (dto == null) return null;

            return new QRs(dto);
        }

        /// <summary>
        /// Front-facing single lookup — returns an InfoDTO, safe to hand straight to a controller/API response.
        /// </summary>
        public static QRModel.QrInfoDTO? GetOneQRCodeBy(
            int? QRCodeID = null, Guid? PublicID = null,
            string? Token = null)
        {
            return QRCodesData.GetOneQRCodeBy(QRCodeID: QRCodeID, PublicID: PublicID, Token: Token);
        }

        /// <summary>
        /// Front-facing list — returns InfoDTOs, safe to hand straight to a controller/API response.
        /// </summary>
        public static List<QRModel.QrInfoDTO> GetAllQRCodes(
            int? UserID = null, int? BreadPointID = null,
            int? Status = null, bool? IsScanned = null,
            int PageNumber = 1, int PageSize = 10)
        {
            return QRCodesData.GetAllQRCodes(UserID: UserID, BreadPointID: BreadPointID, Status: Status, IsScanned: IsScanned, PageNumber: PageNumber, PageSize: PageSize);
        }

        /// <summary>
        /// Internal-only loader — pulls the full ObjDTO so the returned instance is ready
        /// to Save()/Delete().
        /// </summary>
        public static QRs? Find(int QRCodeID)
        {
            var dto = QRCodesData.GetQRCodeObjBy(QRCodeID: QRCodeID);
            if (dto == null) return null;

            return new QRs(dto);
        }
    }
}
