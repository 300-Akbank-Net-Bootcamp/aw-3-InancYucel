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
        var checkIndentity = await dbContext.Set<Address>()
            .Where(x => x.CustomerId == request.Model.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIndentity != null)
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
        var fromdb = await dbContext.Set<Address>().Where(x => x.CustomerId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        //Null validation?
        fromdb.Address1 = request.Model.Address1; 
        fromdb.Address2 = request.Model.Address2;
        fromdb.Country = request.Model.Country;
        fromdb.City = request.Model.City;
        fromdb.County = request.Model.County;
        fromdb.PostalCode = request.Model.PostalCode;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
      public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
      {
          var fromdb = await dbContext.Set<Address>().Where(x => x.CustomerId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromdb == null)
          {
              return new ApiResponse("Record not found");
          }
          fromdb.IsActive = false; //dbContext.Set<Address>().Remove(fromdb); hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}