using BreadApp_DL;
using BreadApp_Struct.Common;
using System.Runtime.CompilerServices;

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
        public byte[] Token { get; set; } = Array.Empty<byte>();
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
            this.Token = Array.Empty<byte>();
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
                ScannedAt: this.ScannedAt,
                Token: this.Token,
                CreatedAt: this.CreatedAt,
                ExpiresAt: this.ExpiresAt,
                IsScanned: this.IsScanned
            );

            return obj;
        }

        private bool _AddQRCode(SessionContextInfo sessionInfo)
        {

            
            this.Token = QRCodesData.CreateQRCode(_ToObjDTO(), sessionInfo);

            return this.Token != Array.Empty<byte>() ;
        }

        private bool _UpdateQRCode(SessionContextInfo sessionInfo)
        {
            return QRCodesData.UpdateQRCode(_ToObjDTO(), sessionInfo);
        }

        public bool Save(SessionContextInfo sessionInfo)
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddQRCode(sessionInfo))
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateQRCode(sessionInfo);
                default:
                    return false;
            }
        }

        public bool Delete(SessionContextInfo sessionInfo, bool HardDelete = false)
        {
            return QRCodesData.DeleteQRCode(this.QRCodeID, sessionInfo, HardDelete);
        }
        public static bool Delete(int QrID , SessionContextInfo sessionInfo, bool HardDelete = false)
        {
            return QRCodesData.DeleteQRCode(QrID , sessionInfo, HardDelete);
        }
        /// <summary>
        /// Scans a QR code at a bread point. Returns the internal BL object (not an InfoDTO)
        /// so the caller can inspect the result and decide what to expose to the front end,
        /// e.g. via ToInfoDTO() after any business-rule adjustments.
        /// </summary>
        public static QRs? Scan(byte[] Token, SessionContextInfo sessionInfo, int BreadPointID)
        {
            var dto = QRCodesData.ScanQRCode(Token, sessionInfo, BreadPointID);
            if (dto == null) return null;

            return new QRs(dto);
        }

        /// <summary>
        /// Front-facing single lookup — returns an InfoDTO, safe to hand straight to a controller/API response.
        /// </summary>
        public static QRModel.QrInfoDTO? GetOneQRCodeBy(
            int? QRCodeID = null, Guid? PublicID = null,
            byte[]? Token = null)
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
        public bool Cancel( SessionContextInfo sessionInfo)
        {
            return QRCodesData.CancelQrCode(this.QRCodeID, sessionInfo);
        }
        public static bool Cancel(int QrCodeID, SessionContextInfo sessionInfo)
        {
            return QRCodesData.CancelQrCode(QrCodeID, sessionInfo);
        }
    }
}
