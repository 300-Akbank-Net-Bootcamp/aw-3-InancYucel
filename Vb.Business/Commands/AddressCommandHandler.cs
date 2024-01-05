using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Commands;

public class AddressCommandHandler :
    IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
    IRequestHandler<UpdateAddressCommand, ApiResponse>,
    IRequestHandler<DeleteAddressCommand, ApiResponse>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AddressCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        /* Controls */
        var checkIdentity = await dbContext.Set<Address>()
            .Where(x => x.CustomerId == request.Model.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (checkIdentity != null)
        {
            return new ApiResponse<AddressResponse>($"{request.Model.CustomerId} is used by another Address");
        }
         
        var entity = mapper.Map<AddressRequest, Address>(request.Model);
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Address, AddressResponse>(entityResult.Entity);
        return new ApiResponse<AddressResponse>(mapped); 
    }

    public async Task<ApiResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<Address>().Where(x => x.CustomerId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromDb == null) // Null Controls
        {
            return new ApiResponse("Record not found");
        }
        
        //Empty Input Validations
        if (request.Model.Address1 != "")
        {
            fromDb.Address1 = request.Model.Address1; 
        }
        if (request.Model.Address2 != "")
        {
            fromDb.Address2 = request.Model.Address2;
        }
        if (request.Model.Country != "")
        {
            fromDb.Country = request.Model.Country;
        }
        if (request.Model.City != "")
        {
            fromDb.City = request.Model.City;
        }
        if (request.Model.County != "")
        {
            fromDb.County = request.Model.County;
        }
        if (request.Model.PostalCode != "")
        {
            fromDb.PostalCode = request.Model.PostalCode;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
      public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
      {
          var fromDb = await dbContext.Set<Address>().Where(x => x.CustomerId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromDb == null)
          {
              return new ApiResponse("Record not found");
          }
          fromDb.IsActive = false; //dbContext.Set<Address>().Remove(fromDb); hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}