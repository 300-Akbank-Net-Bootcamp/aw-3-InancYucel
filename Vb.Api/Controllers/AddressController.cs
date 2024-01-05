using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace AkbankBootCampTaskWeek1.Controllers;
    
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator mediator;

        public AddressController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        // GET: api/Address
        [HttpGet]
        public async Task<ApiResponse<List<AddressResponse>>> Get()
        {
            var operation = new GetAllAddressQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        // GET: api/Address/5
        [HttpGet("{id}")]
        public async Task<ApiResponse<AddressResponse>> Get(int id)
        {
            var operation = new GetAddressByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/Address/CustomerId
        [HttpGet("GetIdByRoute/{CustomerId}")]
        public async Task<ApiResponse<List<AddressResponse>>> GetByParameter(int CustomerId)
        {
            var operation = new GetAllAddressByParameterQuery(CustomerId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // POST: api/Address
        [HttpPost]
        public async Task<ApiResponse<AddressResponse>> Post([FromBody] AddressRequest address)
        {
            var operation = new CreateAddressCommand(address);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/Address/5
        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] AddressRequest address)
        {
            var operation = new UpdateAddressCommand(id, address);
            var result = await mediator.Send(operation);
            return result;
        }
        
        
        // DELETE: api/Address/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteAddressCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
