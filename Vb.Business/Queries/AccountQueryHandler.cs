using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Queries;

public class AccountQueryHandler : 
    IRequestHandler<GetAllAccountQuery, ApiResponse<List<AccountResponse>>>,
    IRequestHandler<GetAccountByIdQuery, ApiResponse<AccountResponse>>,
    IRequestHandler<GetAllAccountByParameterQuery, ApiResponse<List<AccountResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;
    
    public AccountQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        var list =  await dbContext.Set<Account>()
            .Include(x => x.Customer)
            .ToListAsync(cancellationToken);
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<AccountResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Account>, List<AccountResponse>>(list);
        return new ApiResponse<List<AccountResponse>>(mappedList);
    }

    public async Task<ApiResponse<AccountResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        //We also added the relevant Customer data into the Account data.
        var entity = await dbContext.Set<Account>()
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.CustomerId == request.Id, cancellationToken);
        
        if (entity == null) //If no result is found according to the entered Id
        {
            return new ApiResponse<AccountResponse>("Record not found");
        }
        
        var mappedAccount = mapper.Map<Account, AccountResponse>(entity);
        return new ApiResponse<AccountResponse>(mappedAccount);
    }

    public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAllAccountByParameterQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Account>()
            .Include(x => x.Customer)
            .Where(x => x.Customer.CustomerNumber == request.CustomerId)
            .ToListAsync(cancellationToken); 
        //CustomerId in the Account table matches CustomerNumber in the Customer table.
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<AccountResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Account>, List<AccountResponse>>(list);
        return new ApiResponse<List<AccountResponse>>(mappedList);
    }
}