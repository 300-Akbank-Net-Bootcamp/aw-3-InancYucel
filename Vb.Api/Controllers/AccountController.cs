using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace AkbankBootCampTaskWeek1.Controllers;
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        // GET: api/Account
        [HttpGet]
        public async Task<ApiResponse<List<AccountResponse>>> Get()
        {
            var operation = new GetAllAccountQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        // GET: api/Account/5
        [HttpGet("{customerId}")]
        public async Task<ApiResponse<AccountResponse>> Get(int customerId)
        {
            var operation = new GetAccountByIdQuery(customerId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/Account/CustomerId
        [HttpGet("GetIdByRoute/{customerId}")]
        public async Task<ApiResponse<List<AccountResponse>>> GetByParameter(int customerId)
        {
            var operation = new GetAllAccountByParameterQuery(customerId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // POST: api/Account
        [HttpPost]
        public async Task<ApiResponse<AccountResponse>> Post([FromBody] AccountRequest account)
        {
            var operation = new CreateAccountCommand(account);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/Account/5
        [HttpPut("{customerId}")]
        public async Task<ApiResponse> Put(int customerId, [FromBody] AccountRequest account)
        {
            var operation = new UpdateAccountCommand(customerId, account);
            var result = await mediator.Send(operation);
            return result;
        }


        // DELETE: api/Account/5
        [HttpDelete("{customerId}")]
        public async Task<ApiResponse> Delete(int customerId)
        {
            var operation = new DeleteAccountCommand(customerId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
