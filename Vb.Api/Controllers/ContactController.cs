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
        [HttpGet("{id}")]
        public async Task<ApiResponse<ContactResponse>> Get(int id)
        {
            var operation = new GetContactByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        // GET: api/Contact/CustomerId
        [HttpGet("GetIdByRoute/{CustomerId}")]
        public async Task<ApiResponse<List<ContactResponse>>> GetByParameter(int CustomerId)
        {
            var operation = new GetAllContactByParameterQuery(CustomerId);
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
        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] ContactRequest contact)
        {
            var operation = new UpdateContactCommand(id, contact);
            var result = await mediator.Send(operation);
            return result;
        }
        
        
        // DELETE: api/Contact/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteContactCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
