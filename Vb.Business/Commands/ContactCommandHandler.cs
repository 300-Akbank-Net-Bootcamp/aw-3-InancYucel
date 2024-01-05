using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Commands;

public class ContactCommandHandler :
    IRequestHandler<CreateContactCommand, ApiResponse<ContactResponse>>,
    IRequestHandler<UpdateContactCommand, ApiResponse>,
    IRequestHandler<DeleteContactCommand, ApiResponse>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public ContactCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<ContactResponse>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        /* Controls */
        var checkIdentity = await dbContext.Set<Contact>()
            .Where(x => x.CustomerId == request.Model.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIdentity != null)
        {
            return new ApiResponse<ContactResponse>($"{request.Model.CustomerId} is used by another Contact");
        }
         
        var entity = mapper.Map<ContactRequest, Contact>(request.Model);
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Contact, ContactResponse>(entityResult.Entity);
        return new ApiResponse<ContactResponse>(mapped); 
    }

    public async Task<ApiResponse> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<Contact>().Where(x => x.CustomerId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromDb == null) // Null Controls
        {
            return new ApiResponse("Record not found");
        }

        //Empty Input Validations
        if (request.Model.ContactType != "")
        {
            fromDb.ContactType = request.Model.ContactType;
        }
        if (request.Model.Information != "")
        {
            fromDb.Information = request.Model.Information;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

      public async Task<ApiResponse> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
      {
          var fromDb = await dbContext.Set<Contact>().Where(x => x.CustomerId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromDb == null) // Null Controls
          {
              return new ApiResponse("Record not found");
          }
          fromDb.IsActive = false; //dbContext.Set<Contact>().Remove(fromDb); hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}