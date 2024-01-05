using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Commands;

//null validation'ları yap

public class CustomerCommandHandler :
    IRequestHandler<CreateCustomerCommand, ApiResponse<CustomerResponse>>,
    IRequestHandler<UpdateCustomerCommand, ApiResponse>,
    IRequestHandler<DeleteCustomerCommand, ApiResponse>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public CustomerCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        
        var checkIdentity = await dbContext.Set<Customer>()
            .Where(x => x.IdentityNumber == request.Model.IdentityNumber)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (checkIdentity != null) // Null Controls
        {
            return new ApiResponse<CustomerResponse>($"{request.Model.IdentityNumber} is used by another Customer");
        }
         
        var entity = mapper.Map<CustomerRequest, Customer>(request.Model);
        
        int customerNumber = new Random().Next(1000000, 9999999);
        
        //Control to ensure that the randomly generated customer Number is not re-added with the same value.
        var any = dbContext.Customers.FirstOrDefault(u => u.CustomerNumber == customerNumber); 
        
        if(any == null)
        {
            return new ApiResponse<CustomerResponse>($"{request.Model.CustomerNumber} is added in the database");
        }
        
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Customer, CustomerResponse>(entityResult.Entity);
        return new ApiResponse<CustomerResponse>(mapped); 
    }

    public async Task<ApiResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<Customer>().Where(x => x.CustomerNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromDb == null) // Null Controls
        {
            return new ApiResponse("Record not found");
        }
        
        //Empty Input Validations
        
        if (request.Model.FirstName != "")
        {
            fromDb.FirstName = request.Model.FirstName;
        }
        
        if (request.Model.LastName != "")
        {
            fromDb.LastName = request.Model.LastName;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
  
    public async Task<ApiResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<Customer>().Where(x => x.CustomerNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken); 
        
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false; //dbContext.Set<Customer>().Remove(fromDb); hard delete
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}