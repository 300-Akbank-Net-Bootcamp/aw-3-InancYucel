using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Commands;

public class AccountCommandHandler :
    IRequestHandler<CreateAccountCommand, ApiResponse<AccountResponse>>,
    IRequestHandler<UpdateAccountCommand, ApiResponse>,
    IRequestHandler<DeleteAccountCommand, ApiResponse>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AccountCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<AccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var checkIdentity = await dbContext.Set<Account>()
            .Where(x => x.CustomerId == request.Model.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (checkIdentity != null) //Null Controls
        {
            return new ApiResponse<AccountResponse>($"{request.Model.CustomerId} is used by another Account");
        }
         
        var entity = mapper.Map<AccountRequest, Account>(request.Model);
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Account, AccountResponse>(entityResult.Entity);
        return new ApiResponse<AccountResponse>(mapped); 
    }

    public async Task<ApiResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<Account>().Where(x => x.CustomerId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fromDb == null) //Null Controls
        {
            return new ApiResponse("Record not found");
        }

        //Empty Input Validations
        if (request.Model.Balance !=  0)
        {
            fromDb.Balance = request.Model.Balance;
        }

        if (request.Model.Name != "")
        {
            fromDb.Name = request.Model.Name;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

      public async Task<ApiResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
      {
          var fromDb = await dbContext.Set<Account>().Where(x => x.CustomerId == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

          if (fromDb == null)
          {
              return new ApiResponse("Record not found");
          }
          fromDb.IsActive = false; //dbContext.Set<Account>().Remove(fromDb); hard delete
          await dbContext.SaveChangesAsync(cancellationToken);
          return new ApiResponse();
      }
}