using System.Net;
using Demo.Models;
using Demo.Models.InputModel;
using Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    
    [Route("api/User")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddUser([FromBody] UserInput users)
        {
            var request = new Users
            {
                FirstName = users.FirstName,
                LastName = users.LastName,
                UserName = users.UserName,
                Email = users.Email,
                Password = users.Password,
                Tel = users.Tel
            };
            var result = _userService.AddUsers(request);
            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetUser()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        [HttpGet]
        [Route("{userName}/login/{pass}")]
        public IActionResult Login([FromRoute]string userName,[FromRoute]string pass)
        {
            var result = _userService.Login(userName, pass);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUserById([FromRoute]int id)
        {
            var result = _userService.GetByIdUsers(id);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateUser([FromRoute]int id, [FromBody]UserInput users)
        {
            var input = new Users
            {
                Id = id,
                UserName = users.UserName,
                Email = users.Email,
                FirstName = users.FirstName,
                Password = users.Password,
                LastName = users.LastName,
                Tel = users.Tel
            };
            var result = _userService.UpdateUser(input);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/password")]
        public IActionResult ChangePass([FromRoute]int id, [FromBody]string pass)
        {
            var result = _userService.ChangePass(pass,id);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser([FromRoute]int id)
        {
            var result = _userService.DeleteUserById(id);
            return Ok(result);
        }
    }
}
