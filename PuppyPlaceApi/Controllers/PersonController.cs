using Microsoft.AspNetCore.Mvc;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlaceApi.Controllers;

[Route("api/[Controller]")]
public class PersonController : Controller
{
    private readonly PersonRepository _personRepository;

    public PersonController(PersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Person>>> PersonsAsync()
    {
        return Ok(await _personRepository.PersonsAsync());
    }

    // localhost/api/Person/{theId}
    
    
    [HttpGet("{theId}")]
    public async Task<ActionResult<Person?>> FindPerson(Guid theId)
    {
       var person = await _personRepository.FindByIdAsync(theId);
       if (person == null)
       {
           return NotFound();
       }

       return person;
    }

    [HttpDelete ("{id}")]
    public async Task<ActionResult> DeletePerson(Guid id)
    {
        await _personRepository.RemovePersonAsync(id);
        return NoContent();
    }
    









    // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }
}