using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Queries;

public class AccountTransactionQueryHandler : 
    IRequestHandler<GetAllAccountTransactionQuery, ApiResponse<List<AccountTransactionResponse>>>,
    IRequestHandler<GetAccountTransactionByIdQuery, ApiResponse<AccountTransactionResponse>>,
     IRequestHandler<GetAllAccountTransactionByParameterQuery, ApiResponse<List<AccountTransactionResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;
    
    public AccountTransactionQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAllAccountTransactionQuery request, CancellationToken cancellationToken)
    {
        var list =  await dbContext.Set<AccountTransaction>()
            .Include(x => x.Account)
            .ToListAsync(cancellationToken);
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<AccountTransactionResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<AccountTransaction>, List<AccountTransactionResponse>>(list);
        return new ApiResponse<List<AccountTransactionResponse>>(mappedList);
    }

    public async Task<ApiResponse<AccountTransactionResponse>> Handle(GetAccountTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        //We also added the relevant Customer data into the AccountTransaction data.
        var entity = await dbContext.Set<AccountTransaction>()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.AccountId == request.Id, cancellationToken);
        
        if (entity == null) //If no result is found according to the entered Id
        {
            return new ApiResponse<AccountTransactionResponse>("Record not found");
        }
        
        var mappedAccountTransaction = mapper.Map<AccountTransaction, AccountTransactionResponse>(entity);
        return new ApiResponse<AccountTransactionResponse>(mappedAccountTransaction);
    }

    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAllAccountTransactionByParameterQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<AccountTransaction>()
            .Include(x => x.Account)
            .Where(x => x.Account.AccountNumber == request.AccountId)
            .ToListAsync(cancellationToken);
        //CustomerId in the AccountTransaction table matches CustomerNumber in the Customer table.

        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise.
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<AccountTransactionResponse>>("Record not found");
        }

        var mappedList = mapper.Map<List<AccountTransaction>, List<AccountTransactionResponse>>(list);
        return new ApiResponse<List<AccountTransactionResponse>>(mappedList);
    }
}