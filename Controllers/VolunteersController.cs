using EventsApi.Models;
using EventsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        private readonly VolunteerService _volunteerService;

        public VolunteersController(VolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
        }

        [HttpGet]
        public ActionResult<List<Volunteer>> Get() =>
            _volunteerService.Get();

        [HttpGet("{id}/{password}")]
        public ActionResult<Volunteer> Get(string id,string password)
        {
            var volunteer = _volunteerService.GetVolunteerByIdAndPassword(id,password);

            if (volunteer == null)
            {
                return NotFound();
            }

            return volunteer;
        }

        // [HttpPost]
        // public ActionResult<Book> Create(Book book)
        // {
        //     _bookService.Create(book);

        //     return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        // }

        // [HttpPut("{id:length(24)}")]
        // public IActionResult Update(string id, Book bookIn)
        // {
        //     var book = _bookService.Get(id);

        //     if (book == null)
        //     {
        //         return NotFound();
        //     }

        //     _bookService.Update(id, bookIn);

        //     return NoContent();
        // }

        // [HttpDelete("{id:length(24)}")]
        // public IActionResult Delete(string id)
        // {
        //     var book = _bookService.Get(id);

        //     if (book == null)
        //     {
        //         return NotFound();
        //     }

        //     _bookService.Remove(book.Id);

        //     return NoContent();
        // }
    }
}