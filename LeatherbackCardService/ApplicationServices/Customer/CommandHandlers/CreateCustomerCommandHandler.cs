using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Customer.Commands;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Extensions;

namespace ApplicationServices.Customer.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(ICardServiceDbContext dbContext,
            ILogger<CreateCustomerCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerIdentity || x.Email == request.Email,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            _logger.LogError($"{nameof(CreateCustomerCommandHandler)} :: Request: {request.ToJson()}");
            
            if (customer is not null && customer.Id != request.CustomerIdentity)
            {
                _logger.LogError($"{nameof(CreateCustomerCommandHandler)} :: Response: Email {request.Email} already exist !");
                return Result.Fail($"Email {request.Email} already exist !");
            }
            

            if (customer is not null)
            {
                await UpdateCustomer(request, customer,cancellationToken);

                _logger.LogInformation($"{nameof(CreateCustomerCommandHandler)} :: Response: {customer.ToJson()}");
                return Result.Ok(customer);
            }
            

            var customerResult = Domain.Entities.Customers.Customer.Create(request.CustomerIdentity,
                request.FirstName, request.LastName, request.Email,
                request.Sex, request.PhoneNumber,
                request.CustomerType, request.CountryIso, request.DateOfBirth, request.Address, request.City,
                request.PostalCode, request.MiddleName, request.companyId);

            if (!customerResult.IsSuccess)
                return Result.Fail(customerResult.Error);

            customer = customerResult.Value;

            customer.Address = request.Address;
            customer.City = request.City;
            customer.MiddleName = request.MiddleName;
            customer.Sex = request.Sex;
            customer.ProductId = request.ProductId;

            await _dbContext.Customers.AddAsync(customer, cancellationToken).ConfigureAwait(false);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CreateCustomerCommandHandler)} :: Error: {e}");
                return Result.Fail(e.Message);
            }

            _logger.LogInformation($"{nameof(CreateCustomerCommandHandler)} :: Response: {customer.ToJson()}");
            return Result.Ok(customer);
        }

        private  async Task UpdateCustomer(CreateCustomerCommand request,
            Domain.Entities.Customers.Customer customer, CancellationToken cancellationToken)
        {
            customer.Address = request.Address;
            customer.City = request.City;
            customer.Email = request.Email;
            customer.Sex = request.Sex;
            customer.CustomerType = request.CustomerType;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.MiddleName = request.MiddleName;
            customer.PhoneNumber = request.PhoneNumber;
            customer.PostalCode = request.PostalCode;

             _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}