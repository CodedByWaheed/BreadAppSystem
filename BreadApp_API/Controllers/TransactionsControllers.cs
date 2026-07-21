using BreadApp_BL;
using BreadApp_DL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BreadApp_DL.TransactionModel;
using static BreadApp_DL.UserModel;

namespace BreadApp_API.Controllers
{
    [Authorize]
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsControllers : ControllerBase
    {
        [HttpGet("GetAll", Name = "GetAllTransactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TransactionInfoDTO>>> GetAllTransactions(
            [FromServices] IAuthorizationService authorizationService, int? SenderUserID,
            int? BreadPointID = null,
            int? TransactionType = null, int? Status = null,
            int PageNumber = 1, int PageSize = 10)
        {
            if (SenderUserID == null)
                return BadRequest("The SenderID Cant Be Emptay");

            var transactionList = Transactions.GetAllTransactions(
                SenderUserID: SenderUserID, BreadPointID: BreadPointID,
                TransactionType: TransactionType, Status: Status,
                PageNumber: PageNumber, PageSize: PageSize);

            //int? ID = transactionList.FirstOrDefault(T => T.SenderUserID == UserID)?.SenderUserID;

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                SenderUserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            return Ok(transactionList);
        }




        [HttpGet("GetOneBy", Name = "GetTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransactionInfoDTO>> GetOneTransactionBy(
            [FromServices] IAuthorizationService authorizationService, int? TransactionID = null, Guid? PublicID = null, int? QRCodeID = null)
        {
            if (TransactionID.HasValue && TransactionID < 1)
                return BadRequest("Transaction ID Cannot be less than 1");
            if (QRCodeID.HasValue && QRCodeID < 1)
                return BadRequest("QRCode ID Cannot be less than 1");

            var transaction = Transactions.GetTransactionBy(
                TransactionID: TransactionID ?? null,
                PublicID: PublicID ?? null,
                QRCodeID: QRCodeID ?? null);

            if (transaction == null)
                return NotFound("Transaction Not Found");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                transaction.SenderUserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            return Ok(transaction);
        }



        //[HttpPost("Add", Name = "AddNewTransaction")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //public async Task<ActionResult<TransactionInfoDTO>> AddNewTransaction(TransactionDataDTO cTransDTO ,[FromServices] IAuthorizationService authorizationService)
        //{
        //    if (cTransDTO == null)
        //        return BadRequest("There is no Data Came");
        //    if (cTransDTO.SenderUserID < 1)
        //        return BadRequest("Sender User ID is required and must be greater than 0.");
        //    if (cTransDTO.ReceiverUserID.HasValue && cTransDTO.ReceiverUserID.Value < 1)
        //        return BadRequest("Receiver User ID is invalid.");
        //    if (cTransDTO.BreadPointID.HasValue && cTransDTO.BreadPointID.Value < 1)
        //        return BadRequest("Bread Point ID is invalid.");
        //    if (cTransDTO.QrID.HasValue && cTransDTO.QrID.Value < 1)
        //        return BadRequest("QR Code ID is invalid.");
        //    if (cTransDTO.Amount <= 0)
        //        return BadRequest("Amount must be greater than 0.");

        //    var authResult = await authorizationService.AuthorizeAsync(
        //    User,
        //    cTransDTO.SenderUserID,
        //    "UserOwnerOrAdmin");

        //    if (!authResult.Succeeded)
        //        return Forbid(); // 403


        //    Transactions transaction = new Transactions(cTransDTO);

        //    if (transaction.Save())
        //    {
        //        var createdDTO = transaction.ToInfoDTO();
        //        return CreatedAtRoute("GetTransaction", new { TransactionID = transaction.TransactionID }, transaction.ToInfoDTO());
        //    }
        //    return BadRequest("Failed to Add Transaction.");
        //}




        //[HttpGet("Confirm/{TransactionID}", Name = "Confirm")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult> Confirm(int TransactionID, [FromServices] IAuthorizationService authorizationService)
        //{
        //    if (TransactionID < 1)
        //        return BadRequest("Transaction ID is Invalid.");

        //    Transactions? transaction = Transactions.Find(TransactionID);
        //    if (transaction == null)
        //        return NotFound("Transaction Not found");
        //    BreadPoints? BP = BreadPoints.Find(BreadPointID: transaction.BreadPointID);
        //    if(BP == null)
        //        return NotFound("BreadPiont in Confirming Not found");

        //    var authResult = await authorizationService.AuthorizeAsync(
        //    User,
        //    BP.UserID,
        //    "UserOwnerOrAdmin");

        //    if (!authResult.Succeeded)
        //        return Forbid(); // 403

        //    if (transaction.Confirm())
        //        return Ok("The Transaction has successfully Confirmed.");

        //    return BadRequest("Failed to Confirm Transaction.");
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("Delete/{TransactionID}", Name = "DeleteTransaction")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult DeleteTransaction(int TransactionID, bool HardDelete = false)
        //{
        //    if (TransactionID < 1)
        //        return BadRequest("Transaction ID Can't Be Less than 1");

        //    Transactions? transaction = Transactions.Find(TransactionID);
        //    if (transaction == null)
        //        return NotFound("Transaction Not found");

        //    if (transaction.Delete(false))
        //        return Ok("Transaction Deleted Successfully");

        //    return BadRequest("Some error Occured.");
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPut("Update", Name = "UpdateTransaction")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //public ActionResult<TransactionInfoDTO> UpdateTransaction(int TransactionID, int? Status = null, string? Notes = null)
        //{
        //    // NOTE: TransactionDataDTO (the front-input DTO) is create-only — it has no
        //    // TransactionID/Status, so there's nothing appropriate to bind an "update" body
        //    // to. Taking the editable fields as plain parameters instead, matching how
        //    // TransactionsData.UpdateTransaction is already shaped.
        //    if (TransactionID < 1)
        //        return BadRequest("Transaction ID is required.");

        //    Transactions? transaction = Transactions.Find(TransactionID);
        //    if (transaction == null)
        //        return NotFound($"Transaction with id {TransactionID} not found.");

        //    if (transaction.Status == Transactions.enStatus.Confirmed)
        //        return BadRequest("Cannot update a confirmed transaction.");

        //    transaction.Status = Status.HasValue ? (Transactions.enStatus)Status.Value : transaction.Status;
        //    transaction.Notes = string.IsNullOrEmpty(Notes) ? transaction.Notes : Notes;

        //    if (transaction.Save())
        //    {
        //        return CreatedAtRoute("GetTransaction", new { TransactionID = transaction.TransactionID }, transaction.ToInfoDTO());
        //    }
        //    return BadRequest("Failed to Update Transaction.");
        //}
    }
}