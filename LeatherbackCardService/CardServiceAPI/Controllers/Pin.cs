using System.Threading.Tasks;
using ApplicationServices.CardManagement.Commands;
using ApplicationServices.CardManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseResponse;

namespace CardServiceAPI.Controllers
{
    /// <summary>
    /// </summary>

    [AllowAnonymous]
    public class Pin : BaseController<CardsController>
    {
        /// <summary>
        /// Change card pin
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("change")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePin([FromBody] ChangeCardPinCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
        
        /// <summary>
        /// Validate card pin
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("validate")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidatePin([FromBody] ValidateCardPinCommand command)
        {
            var result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
    }
}