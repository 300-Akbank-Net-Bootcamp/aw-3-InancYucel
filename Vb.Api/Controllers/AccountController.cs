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
        [HttpGet("{id}")]
        public async Task<ApiResponse<AccountResponse>> Get(int id)
        {
            var operation = new GetAccountByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/Account/CustomerId
        [HttpGet("GetIdByRoute/{CustomerId}")]
        public async Task<ApiResponse<List<AccountResponse>>> GetByParameter(int CustomerId)
        {
            var operation = new GetAllAccountByParameterQuery(CustomerId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // POST: api/Account
        [HttpPost]
        public async Task<ApiResponse<AccountResponse>> Post([FromBody] AccountRequest Account)
        {
            var operation = new CreateAccountCommand(Account);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] AccountRequest Account)
        {
            var operation = new UpdateAccountCommand(id, Account);
            var result = await mediator.Send(operation);
            return result;
        }


        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteAccountCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
