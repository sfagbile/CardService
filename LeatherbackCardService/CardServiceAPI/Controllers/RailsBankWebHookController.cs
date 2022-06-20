using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApplicationServices.WebHooks.RailsBank.Commands;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseResponse;
using Shared.Extensions;
using Shared.Utilities;

namespace CardServiceAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [AllowAnonymous]
    public class RailsBankWebHookController : BaseController<RailsBankWebHookController>
    {

        /// <summary>
        /// Rails Bank Notification WebHook
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("notification")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Notification( )
        {
            string body;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }
            
            var command = body.TryDeserializeObject<RailsBankWebHookCommand>(JsonSerializerUtility.SnakeCaseSerializerSettings());
            var  result = await Mediator.Send(command);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
    }
}