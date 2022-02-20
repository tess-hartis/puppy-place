using MediatR;
using Microsoft.AspNetCore.Mvc;
using PuppyPlace.CqrsService.DogCQ.Commands;
using PuppyPlace.CqrsService.DogCQ.Queries;
using PuppyPlace.Domain;
using PuppyPlace.Repository;
using PuppyPlaceApi.Dtos;

namespace PuppyPlaceApi.Controllers;

[Route("api/[Controller]")]

public class DogController : Controller
{
    private readonly IMediator _mediator;

    public DogController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllDogs()
    {
        var query = new GetDogsQuery();
        var result = await _mediator.Send(query);
        return Ok(result.Select(GetDogDto.FromDog));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> FindDogById(Guid id)
    {
        var query = new GetDogByIdQuery(id);
        var dog = await _mediator.Send(query);
        return dog
            .Map(GetDogDto.FromDog)
            .Some<IActionResult>(Ok)
            .None(NotFound);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddDog([FromBody]AddDogCommand request)
    {
        var dog = await _mediator.Send(request);
        return dog.Match<IActionResult>(
            t => Ok(),
            e =>
            {
                var errors = e.Select(e => e.Message).ToList();
                return UnprocessableEntity(new {errors});
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDog(Guid id, [FromBody] UpdateDogCommand request)
    {
        request.Id = id;
        var dog = await _mediator.Send(request);
        return dog
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
    public async Task<IActionResult> DeleteDog(Guid id)
    {
        var command = new DeleteDogCommand(id);
        var result = await _mediator.Send(command);
        return result
            .Some<IActionResult>(_ => NoContent())
            .None(NotFound);
    }

    [HttpPost("{dogId}/owner/{ownerId}")]
    public async Task<IActionResult> AddOwner(Guid dogId, Guid ownerId)
    {
        var command = new AddOwnerCommand(dogId, ownerId);
        var result = await _mediator.Send(command);
        return result
            .Some<IActionResult>(u => Ok())
            .None(NotFound);
    }
}