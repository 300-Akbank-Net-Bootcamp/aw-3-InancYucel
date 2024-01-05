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
        [HttpGet("{id}")]
        public async Task<ApiResponse<AccountTransactionResponse>> Get(int id)
        {
            var operation = new GetAccountTransactionByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/AccountTransaction/CustomerId
        [HttpGet("GetIdByRoute/{AccountId}")]
        public async Task<ApiResponse<List<AccountTransactionResponse>>> GetByParameter(int AccountId)
        {
            var operation = new GetAllAccountTransactionByParameterQuery(AccountId);
            var result = await mediator.Send(operation);
            return result;
        }

        // POST: api/AccountTransaction
        [HttpPost]
        public async Task<ApiResponse<AccountTransactionResponse>> Post([FromBody] AccountTransactionRequest AccountTransaction)
        {
            var operation = new CreateAccountTransactionCommand(AccountTransaction);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/AccountTransaction/5
        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] AccountTransactionRequest AccountTransaction)
        {
            var operation = new UpdateAccountTransactionCommand(id, AccountTransaction);
            var result = await mediator.Send(operation);
            return result;
        }


        // DELETE: api/AccountTransaction/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteAccountTransactionCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
