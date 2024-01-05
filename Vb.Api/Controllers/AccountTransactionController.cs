using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace AkbankBootCampTaskWeek1.Controllers;
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountTransactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        // GET: api/AccountTransaction
        [HttpGet]
        public async Task<ApiResponse<List<AccountTransactionResponse>>> Get()
        {
            var operation = new GetAllAccountTransactionQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        // GET: api/AccountTransaction/5
        [HttpGet("{accountId}")]
        public async Task<ApiResponse<AccountTransactionResponse>> Get(int accountId)
        {
            var operation = new GetAccountTransactionByIdQuery(accountId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/AccountTransaction/CustomerId
        [HttpGet("GetIdByRoute/{accountId}")]
        public async Task<ApiResponse<List<AccountTransactionResponse>>> GetByParameter(int accountId)
        {
            var operation = new GetAllAccountTransactionByParameterQuery(accountId);
            var result = await mediator.Send(operation);
            return result;
        }

        // POST: api/AccountTransaction
        [HttpPost]
        public async Task<ApiResponse<AccountTransactionResponse>> Post([FromBody] AccountTransactionRequest accountTransaction)
        {
            var operation = new CreateAccountTransactionCommand(accountTransaction);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/AccountTransaction/5
        [HttpPut("{accountId}")]
        public async Task<ApiResponse> Put(int accountId, [FromBody] AccountTransactionRequest accountTransaction)
        {
            var operation = new UpdateAccountTransactionCommand(accountId, accountTransaction);
            var result = await mediator.Send(operation);
            return result;
        }


        // DELETE: api/AccountTransaction/5
        [HttpDelete("{accountId}")]
        public async Task<ApiResponse> Delete(int accountId)
        {
            var operation = new DeleteAccountTransactionCommand(accountId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
