using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Query;
using ApplicationServices.CardManagement.Commands;
using ApplicationServices.CardManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseResponse;

namespace CardServiceAPI.Controllers
{
    /// <summary>
    /// Endpoints for card request
    /// </summary>
    ///
    [AllowAnonymous]
    public class CardsController : BaseController<CardsController>
    {
        /// <summary>
        /// Create new card request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("request")]
        [ProducesResponseType(typeof(Result<CardRequestResponseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardRequestResponseViewModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardRequestResponseViewModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CardRequest([FromBody] CreateCardRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Get card request
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("request")]
        [ProducesResponseType(typeof(Result<GetCardRequestResponseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<GetCardRequestResponseViewModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<GetCardRequestResponseViewModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCardRequest([FromQuery] GetCardRequestQuery query)
        {
            var result = await Mediator.Send(query);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<GetCardViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<GetCardViewModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<GetCardViewModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCard([FromQuery] GetCardQuery query)
        {
            var result = await Mediator.Send(query);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("backoffice")]
        [ProducesResponseType(typeof(Result<GetCardDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<GetCardDetailsViewModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<GetCardDetailsViewModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPartialCard([FromQuery] GetCardDetailsQuery query)
        {
            var result = await Mediator.Send(query);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Get cards by customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCardsByCustomerId(Guid customerId)
        {
            var result = await Mediator.Send(new GetCardByCustomerIdQuery{CustomerId = customerId});
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Query issued cards
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("issued")]
        [ProducesResponseType(typeof(Result<PagedResponse<CardViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PagedResponse<CardViewModel>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<PagedResponse<CardViewModel>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetIssuedCard([FromQuery] GetCardsQuery query)
        {
            var result = await Mediator.Send(query);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Get cards by requestId
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet("request/{requestId}")]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCardsByRequestId(Guid requestId)
        {
            var result = await Mediator.Send(new GetCardByCardRequestIdQuery{CardRequestId = requestId});
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        
        /// <summary>
        /// Approve card request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveCardRequest([FromBody] ApproveCardRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Reject card request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("reject")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectCardRequest([FromBody] RejectCardRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Activate card awaiting activation
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("activate")]
        [ProducesResponseType(typeof(Result<CardActivationResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardActivationResponseModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardActivationResponseModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivateCard([FromBody] CardActivationCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        
        /// <summary>
        /// Close card
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("close")]
        [ProducesResponseType(typeof(Result<CardClosureResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardClosureResponseModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardClosureResponseModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CloseCard([FromBody] CardClosureCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Close card
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("delete")]
        [ProducesResponseType(typeof(Result<CardClosureResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardClosureResponseModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardClosureResponseModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCard([FromBody] DeleteCardsCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// Suspend card
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("suspend")]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SuspendCard([FromBody] CardSuspensionCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Reactivate card
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("reactivate")]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReactivateCardCommand([FromBody] ReactivateCardCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Freeze card
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("freeze")]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FreezeCard([FromBody] FreezeCardCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// unfreeze card
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("unfreeze")]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<CardSuspensionResponseModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnFreezeCard([FromBody] UnfreezeCardCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        

        /// <summary>
        /// Get Card Limit Types
        /// </summary>
        /// <returns></returns>
        [HttpGet("LimitType")]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<List<CardViewModel>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCardLimitType()
        {
            var result = await Mediator.Send(new GetCardLimitTypeQuery());
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

    }
}