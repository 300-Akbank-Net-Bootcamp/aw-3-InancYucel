using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Queries;

public class EftTransactionQueryHandler : 
    IRequestHandler<GetAllEftTransactionQuery, ApiResponse<List<EftTransactionResponse>>>,
    IRequestHandler<GetEftTransactionByIdQuery, ApiResponse<EftTransactionResponse>>,
    IRequestHandler<GetAllEftTransactionByParameterQuery, ApiResponse<List<EftTransactionResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;
    
    public EftTransactionQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetAllEftTransactionQuery request, CancellationToken cancellationToken)
    {
        var list =  await dbContext.Set<EftTransaction>()
            .Include(x => x.Account)
            .ToListAsync(cancellationToken);
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<EftTransactionResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<EftTransaction>, List<EftTransactionResponse>>(list);
        return new ApiResponse<List<EftTransactionResponse>>(mappedList);
    }

    public async Task<ApiResponse<EftTransactionResponse>> Handle(GetEftTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        //We also added the relevant Customer data into the EftTransaction data.
        var entity = await dbContext.Set<EftTransaction>()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.AccountId == request.Id, cancellationToken);
        
        if (entity == null) //If no result is found according to the entered Id
        {
            return new ApiResponse<EftTransactionResponse>("Record not found");
        }
        
        var mappedEftTransaction = mapper.Map<EftTransaction, EftTransactionResponse>(entity);
        return new ApiResponse<EftTransactionResponse>(mappedEftTransaction);
    }

    public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetAllEftTransactionByParameterQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<EftTransaction>()
            .Include(x => x.Account)
            .Where(x => x.Account.AccountNumber == request.AccountId)
            .ToListAsync(cancellationToken);
        //CustomerId in the EftTransaction table matches CustomerNumber in the Customer table.

        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise.
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<EftTransactionResponse>>("Record not found");
        }

        var mappedList = mapper.Map<List<EftTransaction>, List<EftTransactionResponse>>(list);
        return new ApiResponse<List<EftTransactionResponse>>(mappedList);
    }
}