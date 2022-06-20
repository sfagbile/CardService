using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApplicationServices.CardIssuance.Command;
using ApplicationServices.RailsBank.Cards.Command;
using ApplicationServices.RailsBank.Cards.Query;
using ApplicationServices.RailsBank.EndUsers.Command;
using ApplicationServices.RailsBank.EndUsers.Query;
using ApplicationServices.RailsBank.Ledger.Command;
using ApplicationServices.RailsBank.Ledger.Query;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseResponse;

namespace CardServiceAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RailsBankController : BaseController<RailsBankController>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("get-end-user-by-customer-id/{customerId}")]
        [ProducesResponseType(typeof(Result<GetEndUserViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(Result<string>), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetEndUserByCustomerId(Guid customerId)
        {
            var response = await Mediator.Send(new GetRailsBankEndUserByCustomerIdQuery() {CustomerId = customerId});
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("get-ledgers-by-customer-id/{customerId}")]
        [ProducesResponseType(typeof(Result<List<GetLedgerByCustomerIdViewModel>>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(Result<string>), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLedgersByCustomerId(Guid customerId)
        {
            var response = await Mediator.Send(new GetRailsBankLedgersByCustomerIdQuery() {CustomerId = customerId});
            return Ok(response);
        }

        /// <summary>
        /// This endpoint is used to add individual end user
        /// Rails Bank
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///POST /add-Individual-End-User
        ///{
        ///    "person": {
        ///        "full_name": {
        ///            "first_name": "Teslim",
        ///            "middle_name": "John",
        ///            "last_name": "Balogun"
        ///        },
        ///        "email": "teslim.balogun@yahoo.com",
        ///        "date_of_birth": "2021-08-04",
        ///        "telephone": "",
        ///        "address": {
        ///            "address_refinement": "flat 10",
        ///            "address_number": "24",
        ///            "address_street": "Ajah",
        ///            "address_city": "Lagos",
        ///            "address_postal_code": "23401",
        ///            "address_iso_country": "NGA"
        ///        },
        ///        "address_history": [
        ///           {
        ///              "address_refinement": "",
        ///              "address_number": "",
        ///              "address_street": "",
        ///              "address_city": "",
        ///              "address_postal_code": "",
        ///              "address_iso_country": "NG",
        ///              "address_start_date": "2021-08-04",
        ///              "address_end_date": "2021-08-04"
        ///           }
        ///        ],
        ///        "nationality": [
        ///            "Nigerian"
        ///         ],
        ///        "country_of_residence": [
        ///             "NGA"
        ///        ],
        ///        "date_onboarded": "2021-08-04"
        ///    },
        ///    "enduser_meta": {
        ///        "foo": "",
        ///        "our_salesforce_reference": ""
        ///    },
        ///    "CustomerId": "61086ac0-f259-42da-99e8-561e9edc1f30"
        ///}
        ///
        /// </remarks>
        /// <param name="endUser"></param>
        [HttpPost("add-Individual-End-User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAddIndividualEndUser([FromBody] AddIndividualEndUserCommand endUser)
        {
            var response = await Mediator.Send(endUser);
            return Ok(response);
        }

        /// <summary>
        /// This endpoint is used to issue physical card to customer
        /// Rails Bank
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /issue-Card-To-Customer
        /// {
        ///     "Country": "Gb",
        ///     "CustomerId": "61086ac0-f259-42da-99e8-561e9edc1f30",
        ///     "LedgerId": "61086ac0-f259-42da-99e8-561e9edc1f30",
        ///     "CardCarrierType": "standard",
        ///     "CardDeliveryName": "Carrier Recipient Name",
        ///     "CardType": "physical",
        ///     "CardDeliveryMethod": "standard-first-class",
        ///     "CardDeliveryAddress": {
        ///        "AddressRegion": "England",
        ///        "AddressIsoCountry": "GBR",
        ///        "AddressNumber": "35",
        ///        "AddressPostalCode": "WC1R 4AQ",
        ///        "AddressRefinement": "First Floor",
        ///        "AddressStreet": "Eagle Street",
        ///        "AddressCity": "London"
        ///       }
        /// }
        /// </remarks>
        /// <param name="model"></param>
        [HttpPost("issue-physical-Card-To-Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IssuePhysicalCardToCustomer([FromBody] IssuePhysicalCardCommand model)
        {
            var response = await Mediator.Send(model);
            return Ok(response);
        }

        /// <summary>
        /// This endpoint is used to issue virtual card to customer
        /// Rails Bank
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /issue-virtual-Card-To-Customer
        /// {
        ///     "CustomerId": "61086ac0-f259-42da-99e8-561e9edc1f30",
        ///     "LedgerId": "61086ac0-f259-42da-99e8-561e9edc1f30"
        /// }
        /// </remarks>
        /// <param name="model"></param>
        [HttpPost("issue-virtual-Card-To-Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IssueVirtualCardToCustomer([FromBody] IssueVirtualCardCommand model)
        {
            var response = await Mediator.Send(model);
            return Ok(response);
        }

        /// <summary>
        /// This endpoint is used to activate customer card
        /// Rails Bank
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /activate-customer-card
        /// {
        ///     "CardId": "61086ac0-f259-42da-99e8-561e9edc1f30",
        /// }
        /// </remarks>
        /// <param name="model"></param>
        [HttpPost("activate-customer-card")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivateCustomerCard([FromBody] ActivateCardCommand model)
        {
            var response = await Mediator.Send(model);
            return Ok(response);
        }

        /// <summary>
        /// This endpoint is used to create virtual ledger
        /// Rails Bank
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /create-customer-virtual-ledger
        /// {
        ///     "HolderId": "61086ac0-f259-42da-99e8-561e9edc1f30",
        ///     "CustomerId": "61086ac0-f259-42da-99e8-561e9edc1f30",
        ///     "CountryIso3": "gbp"
        /// }
        /// </remarks>
        /// <param name="ledger"></param>
        [HttpPost("create-customer-virtual-ledger")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomerVirtualLedger([FromBody] CreateVirtualLedgerCommand ledger)
        {
            var response = await Mediator.Send(ledger);
            return Ok(response);
        }

        /// <summary>
        /// This endpoint is used to create physical ledger
        /// Rails Bank
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /create-customer-ledger
        /// {
        ///     "HolderId": "61086ac0-f259-42da-99e8-561e9edc1f30",
        ///     "CustomerId": "61086ac0-f259-42da-99e8-561e9edc1f30"
        /// }
        /// </remarks>
        /// <param name="ledger"></param>
        [HttpPost("create-customer-physical-ledger")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomerPhysicalLedger([FromBody] CreatePhysicalLedgerCommand ledger)
        {
            var response = await Mediator.Send(ledger);
            return Ok(response);
        }

        /// <summary>
        /// This endpoint is used to suspend railsbank card
        /// Rails Bank
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /suspend-rails-bank-customer-card
        /// {
        ///     "Reason": "lost card", 
        ///     "CardId": "61086ac0-f259-42da-99e8-561e9edc1f30"
        /// }
        /// </remarks>
        /// <param name="card"></param>
        [HttpPost("suspend-rails-bank-customer-card")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SuspendRailsBankCustomerCard([FromBody] ActivateCardCommand card)
        {
            var response = await Mediator.Send(card);
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet("get-rails-bank-card-by-id/{customerId}")]
        [ProducesResponseType(typeof(Result<GetRailsBankCardByIdResponseViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(Result<string>), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRailsBankCardByCardId(Guid cardId)
        {
            var response = await Mediator.Send(new GetCustomerCardByIdCommand() {CardId = cardId});
            return Ok(response);
        }
    }
}