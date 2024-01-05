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
        /* Controls */
        var checkIndentity = await dbContext.Set<EftTransaction>()
            .Where(x => x.AccountId == request.Model.AccountId)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIndentity != null)
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
        var fromdb = await dbContext.Set<EftTransaction>().Where(x => x.AccountId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }

        //Null validation?
        fromdb.ReferenceNumber = request.Model.ReferenceNumber;
        fromdb.Description = request.Model.Description;
        fromdb.SenderAccount = request.Model.SenderAccount;
        fromdb.SenderBank = request.Model.SenderBank;
        fromdb.SenderName = request.Model.SenderName;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

      public async Task<ApiResponse> Handle(DeleteEftTransactionCommand request, CancellationToken cancellationToken)
      {
          var fromdb = await dbContext.Set<EftTransaction>().Where(x => x.AccountId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromdb == null)
          {
              return new ApiResponse("Record not found");
          }
          fromdb.IsActive = false; //dbContext.Set<EftTransaction>().Remove(fromdb); hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}