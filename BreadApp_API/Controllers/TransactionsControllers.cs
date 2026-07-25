using BreadApp_BL;
using BreadApp_DL;
using BreadApp_Struct.Common;
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
        [HttpGet("All", Name = "GetAllTransactions")]
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




        [HttpGet("By", Name = "GetTransaction")]
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



        [Authorize(Roles = "Admin")]
        [HttpPut("Confirm/{TransactionID}", Name = "ConfirmTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ConfirmTransaction(int TransactionID , [FromServices] SessionContextInfo sessionInfo)
        {
            Access.Insert(sessionInfo);
            if (TransactionID < 1)
                return BadRequest("ID Can't be less than 1");

            Transactions? Trans = Transactions.Find(TransactionID);
            if (Trans == null)
                return BadRequest("Transaction not found");

            Trans.Status = Transactions.enStatus.Confirmed;
          

            if (Trans.Confirm(sessionInfo))
                return Ok("Transaction Confirmed Successfully");

            return BadRequest("Transaction Confirm Failed");

        }

    }
}