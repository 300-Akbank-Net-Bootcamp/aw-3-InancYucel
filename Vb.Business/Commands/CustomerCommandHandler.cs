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
        /** kontroller  = Hoca hepsi için isteyecek dökümana yazarım demiş*/
        var checkIndentity = await dbContext.Set<Customer>()
            .Where(x => x.IdentityNumber == request.Model.IdentityNumber)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIndentity != null)
        {
            return new ApiResponse<CustomerResponse>($"{request.Model.IdentityNumber} is used by another Customer");
        }
         
        var entity = mapper.Map<CustomerRequest, Customer>(request.Model);
        entity.CustomerNumber = new Random().Next(1000000, 9999999); //unique mi veritabanında daha önce oluşmuş mu diye kontrol edilmesi lazım
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Customer, CustomerResponse>(entityResult.Entity);
        return new ApiResponse<CustomerResponse>(mapped); 
    }

    public async Task<ApiResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<Customer>().Where(x => x.CustomerNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }
        //Null Controls?
        fromdb.FirstName = request.Model.FirstName; 
        fromdb.LastName = request.Model.LastName;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
  
    public async Task<ApiResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<Customer>().Where(x => x.CustomerNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken); 
        
        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        //dbContext.Set<Customer>().Remove(fromdb); hard delete
        
        fromdb.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}