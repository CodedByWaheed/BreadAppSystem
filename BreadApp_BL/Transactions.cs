using BreadApp_DL;
using System.Net.Http.Headers;
using System.Transactions;
using static BreadApp_DL.TransactionModel;

namespace BreadApp_BL
{
    public class Transactions
    {
        public enum enStatus { Pending = 1 , Confirmed = 2 , Canceled = 3 }
        public enum enTransactionType { BreadBuying = 1 , Payment = 2 , Refund =3 , TopApp = 4 }
        public enum enMode { Add = 1, Update = 2 }
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

        public Transactions(TransactionModel.TransactionDTO TransDTO , enMode Mode = enMode.Update)
        {
            this.TransactionID = TransDTO.TransactionID;
            this.PublicID = TransDTO.PublicID;
            this.SenderUserID = TransDTO.SenderUserID;
            this.ReceiverUserID = TransDTO.ReceiverUserID;
            this.BreadPointID = TransDTO.BreadPointID;
            this.QRCodeID = TransDTO.QRCodeID;
            this.Amount = TransDTO.Amount;
            this.TransactionType = (enTransactionType)TransDTO.TransactionType;
            this.Status = (enStatus)TransDTO.Status;
            this.CreatedAt = TransDTO.CreatedAt;
            this.ConfirmedAt = TransDTO.ConfirmedAt;
            this.Notes = TransDTO.Notes;
            _Mode = enMode.Update;
        }

        public Transactions(CreateTransactionDTO cTransDTO, enMode Mode = enMode.Add)
        {
            this.TransactionID = null;
            this.PublicID = null;
            this.SenderUserID = cTransDTO.SenderUserID;
            this.ReceiverUserID = cTransDTO.ReceiverUserID;
            this.BreadPointID = cTransDTO.BreadPointID;
            this.QRCodeID = cTransDTO.QRCodeID;
            this.Amount = cTransDTO.Amount;
            this.TransactionType = (enTransactionType)cTransDTO.TransactionType;
            this.Status = enStatus.Pending;
            this.CreatedAt = DateTime.Now;
            this.ConfirmedAt = null;
            this.Notes = cTransDTO.Notes;
            _Mode = Mode;
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
           
            return new Transactions(dto);
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
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddTransaction())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateTransaction();
                default:
                    return false;
            }
      
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

        public static List<TransactionModel.TransactionDTO> GetAllTransactions(
            int? SenderUserID = null, int? BreadPointID = null,
            int? TransactionType = null, int? Status = null,
            int PageNumber = 1, int PageSize = 10)
        {
            return TransactionsData.GetTransactions(
                SenderUserID, BreadPointID, TransactionType, Status, PageNumber, PageSize);
        }



      
    }
}
