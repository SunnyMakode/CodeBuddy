using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBuddy.Api.Context.Repository;
using CodeBuddy.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeBuddy.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;

        public UserController(IGenericRepository genericRepository)
        {
            this._genericRepository = genericRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _genericRepository.GetAll<User>(i => i.Photos);

            return Ok(users);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var users = await _genericRepository.Get<User>(id
                , i => i.Photos
                , i => i.Id == id);

            return Ok(users);
        }        
    }
}
