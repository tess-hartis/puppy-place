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

    
    
    
    
  
    



    // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }
}