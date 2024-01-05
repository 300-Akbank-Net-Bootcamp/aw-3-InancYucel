using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Queries;

public class AddressQueryHandler : 
    IRequestHandler<GetAllAddressQuery, ApiResponse<List<AddressResponse>>>,
    IRequestHandler<GetAddressByIdQuery, ApiResponse<AddressResponse>>,
    IRequestHandler<GetAllAddressByParameterQuery, ApiResponse<List<AddressResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;
    
    public AddressQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAllAddressQuery request, CancellationToken cancellationToken)
    {
        var list =  await dbContext.Set<Address>()
            .Include(x => x.Customer)
            .ToListAsync(cancellationToken);
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<AddressResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Address>, List<AddressResponse>>(list);
        return new ApiResponse<List<AddressResponse>>(mappedList);
    }

    public async Task<ApiResponse<AddressResponse>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        //We also added the relevant Customer data into the address data.
        var entity = await dbContext.Set<Address>()
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity == null) //If no result is found according to the entered Id
        {
            return new ApiResponse<AddressResponse>("Record not found");
        }
        
        var mappedAddress = mapper.Map<Address, AddressResponse>(entity);
        return new ApiResponse<AddressResponse>(mappedAddress);
    }

    public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAllAddressByParameterQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Address>()
            .Include(x => x.Customer)
            .Where(x => x.Customer.CustomerNumber == request.CustomerId)
            .ToListAsync(cancellationToken); 
        //CustomerId in the address table matches CustomerNumber in the Customer table.
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<AddressResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Address>, List<AddressResponse>>(list);
        return new ApiResponse<List<AddressResponse>>(mappedList);
    }
}