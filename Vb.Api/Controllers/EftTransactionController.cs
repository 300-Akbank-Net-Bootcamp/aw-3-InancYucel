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
        [HttpGet("{id}")]
        public async Task<ApiResponse<EftTransactionResponse>> Get(int id)
        {
            var operation = new GetEftTransactionByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/EftTransaction/CustomerId
        [HttpGet("GetIdByRoute/{AccountId}")]
        public async Task<ApiResponse<List<EftTransactionResponse>>> GetByParameter(int AccountId)
        {
            var operation = new GetAllEftTransactionByParameterQuery(AccountId);
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
        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] EftTransactionRequest EftTransaction)
        {
            var operation = new UpdateEftTransactionCommand(id, EftTransaction);
            var result = await mediator.Send(operation);
            return result;
        }


        // DELETE: api/EftTransaction/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteEftTransactionCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
