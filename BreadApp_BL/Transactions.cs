
using BreadApp_DL;
using BreadApp_Struct.Common;
using static BreadApp_DL.TransactionModel;

namespace BreadApp_BL
{
    public class Transactions
    {
        public enum enStatus { Pending = 1, Confirmed = 2, Canceled = 3 }
        public enum enTransactionType { BreadBuying = 1, Payment = 2, Refund = 3, TopApp = 4 ,Withdraw = 5}
        public enum enMode { Add = 1, Update = 2 }
        enMode _Mode;

        public int TransactionID { get; set; } = -1;
        public Guid PublicID { get; set; } = Guid.Empty;
        public int? SenderUserID { get; set; } = null;
        public int? ReceiverUserID { get; set; } = null;
        public int? BreadPointID { get; set; } = null;
        public int? QRCodeID { get; set; } = null;
        public decimal Amount { get; set; } = 0;
        public enTransactionType TransactionType { get; set; } = enTransactionType.BreadBuying;
        public enStatus Status { get; set; } = enStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ConfirmedAt { get; set; } = null;
        public string? Notes { get; set; } = null;

        public Transactions()
        {
            _Mode = enMode.Add;
        }

        /// <summary>
        /// Built from the full internal object (used by Find()), ready for Confirm()/Save()/Delete().
        /// </summary>
        public Transactions(TransactionObjDTO transDTO, enMode Mode = enMode.Update)
        {
            this.TransactionID = transDTO.TransactionID;
            this.PublicID = transDTO.PublicID;
            this.SenderUserID = transDTO.SenderUserID;
            this.ReceiverUserID = transDTO.ReceiverUserID;
            this.BreadPointID = transDTO.BreadPointID;
            this.QRCodeID = transDTO.QRCodeID;
            this.Amount = transDTO.Amount;
            this.TransactionType = (enTransactionType)transDTO.TransactionType;
            this.Status = (enStatus)transDTO.Status;
            this.CreatedAt = transDTO.CreatedAt;
            this.ConfirmedAt = transDTO.ConfirmedAt;
            this.Notes = transDTO.Notes;
            _Mode = Mode;
        }

        /// <summary>
        /// Built from what the front end sent (create-a-transaction request).
        /// </summary>
        public Transactions(TransactionDataDTO cTransDTO, enMode Mode = enMode.Add)
        {
           
            this.SenderUserID = cTransDTO.SenderUserID.HasValue ? cTransDTO.SenderUserID.Value : this.SenderUserID;
            this.ReceiverUserID = cTransDTO.ReceiverUserID.HasValue ? cTransDTO.ReceiverUserID.Value : this.ReceiverUserID;
            this.BreadPointID = cTransDTO.BreadPointID.HasValue ? cTransDTO.BreadPointID.Value : this.BreadPointID;
            this.QRCodeID = cTransDTO.QrID.HasValue ? cTransDTO.QrID.Value : this.QRCodeID;
            this.Amount = cTransDTO.Amount.HasValue ? cTransDTO.Amount.Value : this.Amount;
            this.TransactionType = cTransDTO.TransactionType.HasValue ? (enTransactionType)cTransDTO.TransactionType.Value : this.TransactionType;
            this.Status = enStatus.Pending;
            this.CreatedAt = DateTime.Now;
            this.ConfirmedAt = null;
            this.Notes = cTransDTO.Notes;
            _Mode = Mode;
        }

        /// <summary>
        /// What we hand back to the front end. Only call once the transaction is fully
        /// persisted (fields below are guaranteed non-null at that point).
        /// </summary>
        public TransactionInfoDTO ToInfoDTO()
        {
            return new TransactionInfoDTO(
                this.TransactionID,
                this.PublicID ,
                this.SenderUserID ,
                this.ReceiverUserID,
                this.BreadPointID,
                this.QRCodeID,
                this.Amount,
                (int)this.TransactionType,
                (int)this.Status,
                this.ConfirmedAt,
                this.Notes ?? string.Empty
            );
        }

        /// <summary>
        /// What we send to the DL layer to create the transaction.
        /// </summary>
        private TransactionObjDTO _ToObjDTO()
        {
            return new TransactionObjDTO(
                this.TransactionID, 
                this.PublicID ,
                this.SenderUserID,
                this.ReceiverUserID,
                this.BreadPointID,
                this.QRCodeID,
                this.Amount,
                (int)this.TransactionType,
                (int)this.Status,
                this.CreatedAt,
                this.ConfirmedAt,
                this.Notes ?? string.Empty
            );
        }

        /// <summary>
        /// Internal-only loader — pulls the full ObjDTO so the returned instance is ready
        /// for Confirm()/Save()/Delete().
        /// </summary>
        public static Transactions? Find(int TransactionID)
        {
            var dto = TransactionsData.GetTransactionObjBy(TransactionID: TransactionID);
            if (dto == null) return null;

            return new Transactions(dto);
        }

        private bool _AddTransaction(SessionContextInfo sessionInfo)
        {
            this.TransactionID = TransactionsData.CreateTransaction(_ToObjDTO(), sessionInfo);
            return this.TransactionID > 0;
        }

        private bool _UpdateTransaction(SessionContextInfo sessionInfo)
        {
            if (this.TransactionID < 1 )
                return false;

            return TransactionsData.UpdateTransaction(sessionInfo: sessionInfo,
                TransactionID: this.TransactionID,
                Status: (int)this.Status,
                Notes: this.Notes
            );
        }

        public bool Save(SessionContextInfo sessionInfo)
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddTransaction(sessionInfo))
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateTransaction(sessionInfo);
                default:
                    return false;
            }
        }

        public bool Confirm(SessionContextInfo sessionInfo)
        {
            if (this.TransactionID > 0)
                return TransactionsData.ConfirmTransaction(sessionInfo:sessionInfo, TransactionID:this.TransactionID);

            return false;
        }

        public bool Delete(SessionContextInfo sessionInfo, bool HardDelete = false)
        {
            if (this.TransactionID > 0)
                return TransactionsData.DeleteTransaction(sessionInfo: sessionInfo, TransactionID: this.TransactionID, HardDelete: HardDelete);

            return false;
        }

        /// <summary>
        /// Front-facing single lookup — returns an InfoDTO, safe to hand straight to a controller/API response.
        /// </summary>
        public static TransactionInfoDTO? GetTransactionBy(
            int? TransactionID = null, Guid? PublicID = null,
            int? QRCodeID = null)
        {
            return TransactionsData.GetTransactionBy(TransactionID, PublicID,QRCodeID);
        }

        /// <summary>
        /// Front-facing list — returns InfoDTOs, safe to hand straight to a controller/API response.
        /// </summary>
        public static List<TransactionInfoDTO> GetAllTransactions(
            int? SenderUserID = null, int? BreadPointID = null,
            int? TransactionType = null, int? Status = null,
            int PageNumber = 1, int PageSize = 10)
        {
            return TransactionsData.GetTransactions(SenderUserID:SenderUserID,BreadPointID:BreadPointID
                , TransactionType: TransactionType,Status: Status, pageNumber: PageNumber, pageSize:PageSize);
        }

        public static List<TransactionUserInfoDTO> GetAllTransactionsUserInterface(
           int? SenderUserID = null, int? BreadPointID = null,
           int? TransactionType = null, int? Status = null,
           int PageNumber = 1, int PageSize = 10)
        {
            return TransactionsData.GetTransactionsUserInfo(SenderUserID: SenderUserID, BreadPointID: BreadPointID
                , TransactionType: TransactionType, Status: Status, pageNumber: PageNumber, pageSize: PageSize);
        }
    }
}

