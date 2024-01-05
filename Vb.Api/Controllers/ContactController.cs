using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace AkbankBootCampTaskWeek1.Controllers;
    
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator mediator;

        public ContactController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        // GET: api/Contact
        [HttpGet]
        public async Task<ApiResponse<List<ContactResponse>>> Get()
        {
            var operation = new GetAllContactQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        // GET: api/Contact/5
        [HttpGet("{customerId}")]
        public async Task<ApiResponse<ContactResponse>> Get(int customerId)
        {
            var operation = new GetContactByIdQuery(customerId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/Contact/CustomerId
        [HttpGet("GetIdByRoute/{customerId}")]
        public async Task<ApiResponse<List<ContactResponse>>> GetByParameter(int customerId)
        {
            var operation = new GetAllContactByParameterQuery(customerId);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // POST: api/Contact
        [HttpPost]
        public async Task<ApiResponse<ContactResponse>> Post([FromBody] ContactRequest contact)
        {
            var operation = new CreateContactCommand(contact);
            var result = await mediator.Send(operation);
            return result;
        }

        // PUT: api/Contact/5
        [HttpPut("{customerId}")]
        public async Task<ApiResponse> Put(int customerId, [FromBody] ContactRequest contact)
        {
            var operation = new UpdateContactCommand(customerId, contact);
            var result = await mediator.Send(operation);
            return result;
        }
        
        
        // DELETE: api/Contact/5
        [HttpDelete("{customerId}")]
        public async Task<ApiResponse> Delete(int customerId)
        {
            var operation = new DeleteContactCommand(customerId);
            var result = await mediator.Send(operation);
            return result;
        }
    }