using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Commands;

public class AccountTransactionCommandHandler :
    IRequestHandler<CreateAccountTransactionCommand, ApiResponse<AccountTransactionResponse>>,
    IRequestHandler<UpdateAccountTransactionCommand, ApiResponse>,
    IRequestHandler<DeleteAccountTransactionCommand, ApiResponse>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AccountTransactionCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<AccountTransactionResponse>> Handle(CreateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        /* Controls */
        var checkIdentity = await dbContext.Set<AccountTransaction>()
            .Where(x => x.AccountId == request.Model.AccountId)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIdentity != null)
        {
            return new ApiResponse<AccountTransactionResponse>($"{request.Model.AccountId} is used by another AccountTransaction");
        }
         
        var entity = mapper.Map<AccountTransactionRequest, AccountTransaction>(request.Model);
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<AccountTransaction, AccountTransactionResponse>(entityResult.Entity);
        return new ApiResponse<AccountTransactionResponse>(mapped); 
    }

    public async Task<ApiResponse> Handle(UpdateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<AccountTransaction>().Where(x => x.AccountId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromDb == null) // Null Controls
        {
            return new ApiResponse("Record not found");
        }

        //Empty Input Validations
        if (request.Model.ReferenceNumber != "")
        {
            fromDb.ReferenceNumber = request.Model.ReferenceNumber;
        }

        if (request.Model.Description != "")
        {
            fromDb.Description = request.Model.Description;
        }

        if (request.Model.TransferType != "")
        {
            fromDb.TransferType = request.Model.TransferType;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

      public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
      {
          var fromDb = await dbContext.Set<AccountTransaction>().Where(x => x.AccountId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromDb == null)
          {
              return new ApiResponse("Record not found");
          }
          fromDb.IsActive = false; //dbContext.Set<AccountTransaction>().Remove(fromDb); hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}