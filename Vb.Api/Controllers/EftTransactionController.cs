using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace AkbankBootCampTaskWeek1.Controllers;
    
    [Route("api/[controller]")]
    [ApiController]
    public class EftTransactionController : ControllerBase
    {
        private readonly IMediator mediator;

        public EftTransactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        // GET: api/EftTransaction
        [HttpGet]
        public async Task<ApiResponse<List<EftTransactionResponse>>> Get()
        {
            var operation = new GetAllEftTransactionQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        // GET: api/EftTransaction/5
        [HttpGet("{accountId}")]
        public async Task<ApiResponse<EftTransactionResponse>> Get(int accountId)
        {
            var operation = new GetEftTransactionByIdQuery(accountId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/EftTransaction/CustomerId
        [HttpGet("GetIdByRoute/{accountId}")]
        public async Task<ApiResponse<List<EftTransactionResponse>>> GetByParameter(int accountId)
        {
            var operation = new GetAllEftTransactionByParameterQuery(accountId);
            var result = await mediator.Send(operation);
            return result;
        }

        // POST: api/EftTransaction
        [HttpPost]
        public async Task<ApiResponse<EftTransactionResponse>> Post([FromBody] EftTransactionRequest EftTransaction)
        {
            var operation = new CreateEftTransactionCommand(EftTransaction);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/EftTransaction/5
        [HttpPut("{accountId}")]
        public async Task<ApiResponse> Put(int accountId, [FromBody] EftTransactionRequest EftTransaction)
        {
            var operation = new UpdateEftTransactionCommand(accountId, EftTransaction);
            var result = await mediator.Send(operation);
            return result;
        }


        // DELETE: api/EftTransaction/5
        [HttpDelete("{accountId}")]
        public async Task<ApiResponse> Delete(int accountId)
        {
            var operation = new DeleteEftTransactionCommand(accountId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
