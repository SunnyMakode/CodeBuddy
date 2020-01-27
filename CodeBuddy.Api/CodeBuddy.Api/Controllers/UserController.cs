using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CodeBuddy.Api.Context.Repository;
using CodeBuddy.Api.Dtos;
using CodeBuddy.Api.Helpers;
using CodeBuddy.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeBuddy.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IMapper _mapper;

        public UserController(IGenericRepository genericRepository, IMapper mapper)
        {
            this._genericRepository = genericRepository;
            this._mapper = mapper;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _genericRepository.GetAll<User>(i => i.Photos);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        // GET api/<controller>/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _genericRepository.Get<User>(id
                , i => i.Photos
                , i => i.Id == id);

            var usersToReturn = _mapper.Map<UserForDetailedDto>(user);
            
            return Ok(usersToReturn);
        }  
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _genericRepository.Get<User>(id
                , i => i.Photos
                , i => i.Id == id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _genericRepository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user failed for {id}");
        }

    }
}
