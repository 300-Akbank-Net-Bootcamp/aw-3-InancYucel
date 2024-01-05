using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace AkbankBootCampTaskWeek1.Controllers;
    
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator mediator;

        public CustomersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        // GET: api/Customers
        [HttpGet]
        public async Task<ApiResponse<List<CustomerResponse>>> Get()
        {
            var operation = new GetAllCustomerQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ApiResponse<CustomerResponse>> Get(int id)
        {
            var operation = new GetCustomerByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/Customers/Debreli/Hasan/24604234
        [HttpGet("GetIdByRoute/{FirstName}/{LastName}/{IdentityNumber}/")]
        public async Task<ApiResponse<List<CustomerResponse>>> GetByParameter(string FirstName, string LastName, string IdentityNumber)
        {
            var operation = new GetAllCustomerByParameterQuery(FirstName, LastName, IdentityNumber);
            var result = await mediator.Send(operation);
            return result;
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ApiResponse<CustomerResponse>> Post([FromBody] CustomerRequest customer)
        {
            var operation = new CreateCustomerCommand(customer);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] CustomerRequest customer)
        {
            var operation = new UpdateCustomerCommand(id, customer);
            var result = await mediator.Send(operation);
            return result;
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteCustomerCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }