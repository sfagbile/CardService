using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Customer.Commands;
using ApplicationServices.Customer.Model;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.CardDetailServices;
using Domain.Entities.Cards;
using Domain.Entities.Customers;
using Domain.Entities.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.BaseResponse;

namespace ApplicationServices.Customer.CommandHandlers
{
    public class
        CreateLedgerCommandHandler : IRequestHandler<CreateLedgerCommand, Result<CreateCardDetailResponseModel>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<ICardDetailService> _customerCardDetailStrategyProcessor;

        public CreateLedgerCommandHandler(
            IStrategyResolver<ICardDetailService> customerCardDetailStrategyProcessor,
            ICardServiceDbContext dbContext)
        {
            _customerCardDetailStrategyProcessor = customerCardDetailStrategyProcessor;
            _dbContext = dbContext;
        }

        public async Task<Result<CreateCardDetailResponseModel>> Handle(CreateLedgerCommand request,
            CancellationToken cancellationToken)
        {
            var createCustomerCardDetailResponse = new CreateCardDetailResponseModel();
            var customerCardDetail = await GetCustomerCardDetail(request, cancellationToken);

            var customerCardDetailStrategyProcessor =
                _customerCardDetailStrategyProcessor.GetService(customerCardDetail.ProviderEndUser.CardProvider.Name);

            if (customerCardDetailStrategyProcessor.HasCreateLedger is false)
            {
                return Result.Ok(new CreateCardDetailResponseModel
                {
                    CardDetailId = customerCardDetail.Id,
                    Status = RequestStatus.Completed,
                });
            }

            var createLedgerResult =
                await customerCardDetailStrategyProcessor.CreateLedger(customerCardDetail, cancellationToken);

            if (createLedgerResult.IsSuccess is false)
            {
                customerCardDetail.Status = CardRequestStatus.Failed;
                _dbContext.CardDetails.Update(customerCardDetail);

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Result.Fail(new CreateCardDetailResponseModel
                {
                    Message = createLedgerResult.Value.Message,
                    ProviderResponse = createLedgerResult.Value.ProviderResponse,
                    Status = RequestStatus.Failed
                }, createLedgerResult.Message, "");
            }

            if (string.IsNullOrWhiteSpace(createLedgerResult.Value.LedgerId) is false)
                customerCardDetail.ProviderLedgerId = createLedgerResult.Value.LedgerId;

            customerCardDetail.Status = customerCardDetailStrategyProcessor.HasCreateLedgerWebHook is true
                ? CardRequestStatus.Inprogress
                : CardRequestStatus.Completed;

            _dbContext.CardDetails.Update(customerCardDetail);
            await _dbContext.SaveChangesAsync(cancellationToken);

            createCustomerCardDetailResponse.Status =
                customerCardDetailStrategyProcessor.HasCreateEndUserWebHook is true
                    ? RequestStatus.Inprogress
                    : RequestStatus.Completed;

            createCustomerCardDetailResponse.ProviderResponse = createLedgerResult.Value.ProviderResponse;
            createCustomerCardDetailResponse.CardDetailId = customerCardDetail.Id;

            return Result.Ok(createCustomerCardDetailResponse);
        }

        private async Task<CardDetail> GetCustomerCardDetail(CreateLedgerCommand request,
            CancellationToken cancellationToken)
        {
            var customerCardDetail = await _dbContext.CardDetails
                .Include(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                
                .Include(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                
                .Include(x => x.Currency)
                .Include(x => x.CardRequest)
                .FirstOrDefaultAsync(x => x.CardRequest.Id == request.CardRequestId,
                    cancellationToken: cancellationToken);
            return customerCardDetail;
        }
    }
}