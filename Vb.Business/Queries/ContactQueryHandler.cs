using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Queries;

public class ContactQueryHandler : 
    IRequestHandler<GetAllContactQuery, ApiResponse<List<ContactResponse>>>,
    IRequestHandler<GetContactByIdQuery, ApiResponse<ContactResponse>>,
    IRequestHandler<GetAllContactByParameterQuery, ApiResponse<List<ContactResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;
    
    public ContactQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<List<ContactResponse>>> Handle(GetAllContactQuery request, CancellationToken cancellationToken)
    {
        var list =  await dbContext.Set<Contact>()
            .Include(x => x.Customer)
            .ToListAsync(cancellationToken);
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<ContactResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Contact>, List<ContactResponse>>(list);
        return new ApiResponse<List<ContactResponse>>(mappedList);
    }

    public async Task<ApiResponse<ContactResponse>> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        //We also added the relevant Customer data into the Contact data.
        var entity = await dbContext.Set<Contact>()
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity == null) //If no result is found according to the entered Id
        {
            return new ApiResponse<ContactResponse>("Record not found");
        }
        
        var mappedContact = mapper.Map<Contact, ContactResponse>(entity);
        return new ApiResponse<ContactResponse>(mappedContact);
    }

    public async Task<ApiResponse<List<ContactResponse>>> Handle(GetAllContactByParameterQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Contact>()
            .Include(x => x.Customer)
            .Where(x => x.Customer.CustomerNumber == request.CustomerId)
            .ToListAsync(cancellationToken); 
        //CustomerId in the Contact table matches CustomerNumber in the Customer table.
        
        if (!list.Any()) //Returns true if there is at least 1 item in the list, false otherwise. 
        {
            //If no result is found according to the entered Id
            return new ApiResponse<List<ContactResponse>>("Record not found");
        }
        
        var mappedList = mapper.Map<List<Contact>, List<ContactResponse>>(list);
        return new ApiResponse<List<ContactResponse>>(mappedList);
    }
}