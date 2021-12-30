using Microsoft.AspNetCore.Mvc;
using PuppyPlace.Domain;
using PuppyPlace.Repository;
using PuppyPlace.Services;

namespace PuppyPlaceApi.Controllers;

[Route("api/[Controller]")]
public class PersonController : Controller
{
    private readonly PersonRepository _personRepository;
    private readonly PersonService _personService;

    public PersonController(PersonRepository personRepository, PersonService personService)
    {
        _personRepository = personRepository;
        _personService = personService;
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
    
    [HttpPost]
    public async Task<ActionResult> AddPerson([FromBody]Person person)
    {
        // if (ModelState.IsValid)
        // {
        //     await _personRepository.AddPersonAsync(person);
        //     return new OkResult();  
        // }
        //
        // return BadRequest();

        await _personService.Validate(person);
        return Ok();

    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePerson(Guid id, [FromBody]Person person)
    {
        var foundPerson = await _personRepository.FindByIdAsync(id);
        foundPerson.Name = person.Name;
        await _personRepository.UpdateNameAsync(foundPerson);
        return Ok();
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