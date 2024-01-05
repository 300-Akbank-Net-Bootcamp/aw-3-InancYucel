using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Commands;

public class EftTransactionCommandHandler :
    IRequestHandler<CreateEftTransactionCommand, ApiResponse<EftTransactionResponse>>,
    IRequestHandler<UpdateEftTransactionCommand, ApiResponse>,
    IRequestHandler<DeleteEftTransactionCommand, ApiResponse>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public EftTransactionCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<EftTransactionResponse>> Handle(CreateEftTransactionCommand request, CancellationToken cancellationToken)
    {
        var checkIdentity = await dbContext.Set<EftTransaction>()
            .Where(x => x.AccountId == request.Model.AccountId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (checkIdentity != null) // Null Controls
        {
            return new ApiResponse<EftTransactionResponse>($"{request.Model.AccountId} is used by another EftTransaction");
        }
         
        var entity = mapper.Map<EftTransactionRequest, EftTransaction>(request.Model);
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<EftTransaction, EftTransactionResponse>(entityResult.Entity);
        return new ApiResponse<EftTransactionResponse>(mapped); 
    }

    public async Task<ApiResponse> Handle(UpdateEftTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<EftTransaction>().Where(x => x.AccountId == request.Id)
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
        if (request.Model.SenderAccount != "")
        {
            fromDb.SenderAccount = request.Model.SenderAccount;
        }
        if (request.Model.SenderBank != "")
        {
            fromDb.SenderBank = request.Model.SenderBank;
        }
        if (request.Model.SenderName != "")
        {
            fromDb.SenderName = request.Model.SenderName;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

      public async Task<ApiResponse> Handle(DeleteEftTransactionCommand request, CancellationToken cancellationToken)
      {
          var fromDb = await dbContext.Set<EftTransaction>().Where(x => x.AccountId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromDb == null) // Null Controls
          {
              return new ApiResponse("Record not found");
          }
          fromDb.IsActive = false; //dbContext.Set<EftTransaction>().Remove(fromDb); To hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}