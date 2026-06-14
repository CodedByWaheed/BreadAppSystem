using BreadApp_DL;
using System.Net.Http.Headers;
using System.Transactions;

namespace BreadApp_BL
{
    public class Transactions
    {
        public enum enStatus { Pending = 1 , Confirmed = 2 , Canceled = 3 }
        public enum enTransactionType { BreadBuying = 1 , Payment = 2 , Refund =3 , TopApp = 4 }
        enum enMode { Add = 1, Update = 2 }
        enMode _Mode;

        public int? TransactionID { get; set; }
        public Guid? PublicID { get; set; }
        public int? SenderUserID { get; set; }
        public int? ReceiverUserID { get; set; }
        public int? BreadPointID { get; set; }
        public int? QRCodeID { get; set; }
        public decimal? Amount { get; set; }
        public enTransactionType? TransactionType { get; set; }
        public enStatus? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public string? Notes { get; set; }

        public Transactions()
        {
            TransactionID = null;
            PublicID = null;
            SenderUserID = null;
            ReceiverUserID = null;
            BreadPointID = null;
            QRCodeID = null;
            Amount = null;
            TransactionType = null;
            Status = enStatus.Pending;
            CreatedAt = null;
            ConfirmedAt = null;
            Notes = null;
            _Mode = enMode.Add;
        }

        private Transactions(int TransactionID, Guid PublicID, int SenderUserID,
            int? ReceiverUserID, int? BreadPointID, int? QRCodeID,
            decimal Amount, enTransactionType? TransactionType, enStatus? Status,
            DateTime CreatedAt, DateTime? ConfirmedAt, string? Notes)
        {
            this.TransactionID = TransactionID;
            this.PublicID = PublicID;
            this.SenderUserID = SenderUserID;
            this.ReceiverUserID = ReceiverUserID;
            this.BreadPointID = BreadPointID;
            this.QRCodeID = QRCodeID;
            this.Amount = Amount;
            this.TransactionType = TransactionType;
            this.Status = Status;
            this.CreatedAt = CreatedAt;
            this.ConfirmedAt = ConfirmedAt;
            this.Notes = Notes;
            _Mode = enMode.Update;
        }

        private TransactionModel.TransactionDTO _ToDTO()
        {
            return new TransactionModel.TransactionDTO(
                this.TransactionID,
                this.PublicID,
                this.SenderUserID,
                this.ReceiverUserID,
                this.BreadPointID,
                this.QRCodeID,
                this.Amount,
                (int)this.TransactionType,
                (int)this.Status,
                this.CreatedAt,
                this.ConfirmedAt,
                this.Notes
            );
        }

        public static Transactions? Find(int TransactionID)
        {
            var dto = TransactionsData.GetTransactionBy(TransactionID: TransactionID);
            if (dto == null) return null;
           
            return new Transactions(
                dto.TransactionID!.Value,
                dto.PublicID!.Value,
                dto.SenderUserID!.Value,
                dto.ReceiverUserID,
                dto.BreadPointID,
                dto.QRCodeID,
                dto.Amount!.Value,
                (enTransactionType)dto.TransactionType,
                (enStatus)dto.Status,
                dto.CreatedAt!.Value,
                dto.ConfirmedAt,
                dto.Notes
            );
        }

        private bool _AddTransaction()
        {
            this.TransactionID = TransactionsData.CreateTransaction(_ToDTO());
            return TransactionID!.Value > 0;
        }

        private bool _UpdateTransaction()
        {
            return TransactionsData.UpdateTransaction(
                this.TransactionID!.Value,
                (int)this.Status,
                this.Notes
            );
        }

        public bool Save()
        {
            if (_Mode == enMode.Add)
                if (_AddTransaction())
                {
                    _Mode = enMode.Update;
                    return true;
                }
            else if (_Mode == enMode.Update)
                return _UpdateTransaction();

            return false;
        }

        public bool Confirm()
        {
            if (this.TransactionID.HasValue)
                return TransactionsData.ConfirmTransaction(this.TransactionID.Value);

            return false;
        }
        public static bool Confirm(int? TransactionID)
        {
            if (TransactionID!.Value < 1)
                return false;
            if (TransactionID.HasValue)
                return TransactionsData.ConfirmTransaction(TransactionID!.Value);
            return false;
        }

        public bool Delete(bool HardDelete = false)
        {
            if (this.TransactionID.HasValue)
                return TransactionsData.DeleteTransaction(this.TransactionID.Value, HardDelete);

            return false;
        }


        public static TransactionModel.TransactionDTO? GetTransactionBy(
            int? TransactionID = null, Guid? PublicID = null,
            int? SenderUserID = null, int? ReceiverUserID = null,
            int? BreadPointID = null, int? QRCodeID = null,
            string? TransactionType = null, string? Status = null)
        {
            return TransactionsData.GetTransactionBy(
                TransactionID, PublicID, SenderUserID, ReceiverUserID,
                BreadPointID, QRCodeID, TransactionType, Status);
        }

        public static TransactionModel.TransactionDTO? GetTransactionByID(int TransactionID)
        {
            return TransactionsData.GetTransactionBy(TransactionID: TransactionID);
        }

        public static TransactionModel.TransactionDTO? GetTransactionByPublicID(Guid PublicID)
        {
            return TransactionsData.GetTransactionBy(PublicID: PublicID);
        }

        public static TransactionModel.TransactionDTO? GetTransactionByQRCode(int QRCodeID)
        {
            return TransactionsData.GetTransactionBy(QRCodeID: QRCodeID);
        }

        public static List<TransactionModel.TransactionDTO> GetAllTransactions(
            int? SenderUserID = null, int? BreadPointID = null,
            string? TransactionType = null, string? Status = null,
            int PageNumber = 1, int PageSize = 10)
        {
            return TransactionsData.GetTransactions(
                SenderUserID, BreadPointID, TransactionType, Status, PageNumber, PageSize);
        }

        public static List<TransactionModel.TransactionDTO> GetTransactionsByUser(
            int SenderUserID, int PageNumber = 1, int PageSize = 10)
        {
            return TransactionsData.GetTransactions(
                SenderUserID: SenderUserID, pageNumber: PageNumber, pageSize: PageSize);
        }

        public static List<TransactionModel.TransactionDTO> GetPendingTransactions(
            int PageNumber = 1, int PageSize = 10)
        {
            return TransactionsData.GetTransactions(
                Status: "Pending", pageNumber: PageNumber, pageSize: PageSize);
        }

        public static List<TransactionModel.TransactionDTO> GetTransactionsByBreadPoint(
            int BreadPointID, int PageNumber = 1, int PageSize = 10)
        {
            return TransactionsData.GetTransactions(
                BreadPointID: BreadPointID, pageNumber: PageNumber, pageSize: PageSize);
        }

        

      
    }
}
