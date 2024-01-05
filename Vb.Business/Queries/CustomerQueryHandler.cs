using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Queries;

public class CustomerQueryHandler :
    IRequestHandler<GetAllCustomerQuery, ApiResponse<List<CustomerResponse>>>,
    IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerResponse>>,
    IRequestHandler<GetAllCustomerByParameterQuery, ApiResponse<List<CustomerResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public CustomerQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetAllCustomerQuery request,
        CancellationToken cancellationToken)
    {
        //We added Account, Contact and Address data to the Customer data
        var list = await dbContext.Set<Customer>() 
            .Include(x => x.Accounts)
            .Include(x => x.Contacts)
            .Include(x => x.Addresses)
            .ToListAsync(cancellationToken);

        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<CustomerResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Customer>, List<CustomerResponse>>(list); //We mapped our Customer data to CustomerResponds
        return new ApiResponse<List<CustomerResponse>>(mappedList); //Encapsulating with ApiResponse and return
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        //We added Account, Contact and Address data to the Customer data
        var entity = await dbContext.Set<Customer>()
            .Include(x => x.Accounts)
            .Include(x => x.Contacts)
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.CustomerNumber == request.Id, cancellationToken);

        if (entity == null) //If no result is found according to the entered Id
        {
            return new ApiResponse<CustomerResponse>("Record not found");
        }

        var mappedCustomer = mapper.Map<Customer, CustomerResponse>(entity);
        return new ApiResponse<CustomerResponse>(mappedCustomer);
    }

    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetAllCustomerByParameterQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Customer>()
            .Include(x => x.Accounts)
            .Include(x => x.Contacts)
            .Include(x => x.Addresses)
            .Where(x => x.FirstName.ToUpper().Contains(request.FirstName.ToUpper())
                        || x.LastName.ToUpper().Contains(request.LastName.ToUpper())
                        || x.IdentityNumber.ToUpper().Contains(request.IdentityNumber.ToUpper()))
            .ToListAsync(cancellationToken);

        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<CustomerResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Customer>, List<CustomerResponse>>(list);
        return new ApiResponse<List<CustomerResponse>>(mappedList);
    }
}