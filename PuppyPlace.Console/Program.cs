using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PuppyPlace.CqrsService;
using PuppyPlace.Data;

namespace PuppyPlace.Console
{
    class Program
    {
        
       static async Task Main(string[] args)
       {
           await Prompt.MainMenu();
       }
       
    }
}