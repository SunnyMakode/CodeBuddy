﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CodeBuddy.Api.Context.Repository;
using CodeBuddy.Api.Dtos;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _genericRepository.Get<User>(id
                , i => i.Photos
                , i => i.Id == id);

            return Ok(user);
        }        
    }
}
