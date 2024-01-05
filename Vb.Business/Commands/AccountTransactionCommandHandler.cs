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
        var checkIndentity = await dbContext.Set<AccountTransaction>()
            .Where(x => x.AccountId == request.Model.AccountId)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIndentity != null)
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
        var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.AccountId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }

        //Null validation?
        fromdb.ReferenceNumber = request.Model.ReferenceNumber;
        fromdb.Description = request.Model.Description;
        fromdb.TransferType = request.Model.TransferType;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

      public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
      {
          var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.AccountId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromdb == null)
          {
              return new ApiResponse("Record not found");
          }
          fromdb.IsActive = false; //dbContext.Set<AccountTransaction>().Remove(fromdb); hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}