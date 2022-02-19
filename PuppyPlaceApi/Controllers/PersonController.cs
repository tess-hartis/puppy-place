using MediatR;
using Microsoft.AspNetCore.Mvc;
using PuppyPlace.CqrsService.PersonCQ.Commands;
using PuppyPlace.CqrsService.PersonCQ.Queries;
using PuppyPlace.Domain;
using PuppyPlace.Repository;


namespace PuppyPlaceApi.Controllers;

[Route("api/[Controller]")]
public class PersonController : Controller
{
    private readonly IMediator _mediator;

    public PersonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        var query = new GetPersonsQuery();
        var result = await _mediator.Send(query);
        return Ok(result.Select(GetPersonDTO.FromPerson));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindPersonById(Guid id)
    {
        var query = new GetPersonByIdQuery(id);
        var person = await _mediator.Send(query);
        return person
            .Map(GetPersonDTO.FromPerson)
            .Some<IActionResult>(Ok)
            .None(NotFound);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] AddPersonCommand request)
    {
        var person = await _mediator.Send(request);
        return person.Match<IActionResult>(
            p => Ok(),
            e =>
            {
                var errors = e.Select(e => e.Message).ToList();
                return UnprocessableEntity(new {errors});
            });
        
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] UpdatePersonCommand request)
    {
        request.Id = id;
        var person = await _mediator.Send(request);
        return person
            .Some(x =>
                x.Succ<IActionResult>(t => Ok())
                    .Fail(e =>
                    {
                        var errors = e.Select(x => x.Message).ToList();
                        return UnprocessableEntity(new {errors});
                    }))
            .None(NotFound);
    }
    
    [HttpDelete ("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var command = new DeletePersonCommand(id);
        var result = await _mediator.Send(command);
        return result
            .Some<IActionResult>(_ => NoContent())
            .None(NotFound);
    }




    // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }
}