using EventsApi.Models;
using EventsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers() =>
            _userService.GetAllUsers();

        [HttpGet("{id:double}")]
        public ActionResult<User> GetUserById(double id,string password)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            _userService.CreateUser(user);
            return user;
            //return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:double}")]
        public IActionResult Update(double id, User updatedUser)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.UpdateUser(id, updatedUser);

            return NoContent();
        }

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