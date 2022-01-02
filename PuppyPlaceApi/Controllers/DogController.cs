using Microsoft.AspNetCore.Mvc;
using PuppyPlace.Domain;
using PuppyPlace.Repository;
using PuppyPlace.Services;

namespace PuppyPlaceApi.Controllers;

[Route("api/[Controller]")]

public class DogController : Controller
{
    private readonly IDogRepository _dogRepository;
    private readonly DogService _dogService;

    public DogController(DogRepository dogRepository, DogService dogService)
    {
        _dogRepository = dogRepository;
        _dogService = dogService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Dog>>> DogsAsync()
    {
        return Ok(await _dogRepository.DogsAsync());
    }
    
    
    
    [HttpGet("{theId}")]
    public async Task<ActionResult<Dog?>> FindDog(Guid theId)
    {
        var dog = await _dogRepository.FindByIdAsync(theId);
        if (dog == null)
        {
            return NotFound();
        }

        return dog;
    }
    
    [HttpPost]
    public async Task<ActionResult> AddDog([FromBody]Dog dog)
    {
        await _dogService.ValidateDog(dog);
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateDog(Guid id, [FromBody]Dog dog)
    {
        var foundDog = await _dogRepository.FindByIdAsync(id);
        foundDog.Name = dog.Name;
        foundDog.Age = dog.Age;
        foundDog.Breed = dog.Breed;
        await _dogService.ValidateUpdatedDog(dog);
        return Ok();
    }


    [HttpDelete ("{id}")]
    public async Task<ActionResult> DeleteDog(Guid id)
    {
        await _dogRepository.RemoveDogAsync(id);
        return NoContent();
    }
}