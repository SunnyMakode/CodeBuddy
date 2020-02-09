using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            Expression<Func<User, bool>> lambdaExpression = null;
            Expression<Func<User, bool>> temporaryPredicate = null;

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _genericRepository.Get<User>(currentUserId
                , i => i.Photos
                , i => i.Id == currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male"
                    ? "female"
                    : "male";
            }

            Expression<Func<User, bool>> predicate1 = (u => u.Id != userParams.UserId && u.Gender == userParams.Gender);

            if (userParams.Followers)
            {
                var userFollowers = await _genericRepository.GetUserFollower(userParams.UserId, userParams.Followers);

                temporaryPredicate = (u => userFollowers.Contains(u.Id));
            }

            if (userParams.Followings)
            {
                var userFollowings = await _genericRepository.GetUserFollower(userParams.UserId, userParams.Followers);

                temporaryPredicate = (u => userFollowings.Contains(u.Id));
            }

            lambdaExpression = temporaryPredicate != null ? predicate1.And(temporaryPredicate) : predicate1;

            //if (predicate1 != null)
            //{
            //    lambdaExpression = temporaryPredicate != null ? predicate1.And(temporaryPredicate) : predicate1;
            //}
            //else if(temporaryPredicate != null)
            //{
            //    lambdaExpression = temporaryPredicate;
            //}
            

            DateTime minDob = new DateTime();
            DateTime maxDob = new DateTime();

            Expression<Func<User, bool>> predicate2 = null;

            if (userParams.MinAge !=18 || userParams.MaxAge !=99)
            {
                minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                predicate2 = (u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }
            
            if (predicate2 != null)
            {
                //lambdaExpression = Expression.Lambda<Func<User, bool>>(Expression.And(predicate1.Body, predicate2.Body),
                //    predicate1.Parameters.Single());

                lambdaExpression = predicate1.And(predicate2);
            }
            

            var users = await _genericRepository.GetAll<User>(i => i.Photos, 
                userParams, lambdaExpression);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

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

        [HttpPost("{id}/follow/{recipientId}")]
        public async Task<IActionResult> FollowUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userConnection = await _genericRepository.GetConnection<Follow>
                                        ( 
                                          id,
                                          recipientId,
                                          u => u.FollowerId == id && u.FollowingId == recipientId
                                        );

            if (userConnection != null)
            {
                return BadRequest("You already follow this user");
            }

            if (await _genericRepository.Get<User>(recipientId) == null)
            {
                return NotFound();
            }

            userConnection = new Follow
            {
                FollowerId = id,
                FollowingId = recipientId
            };

            _genericRepository.Add<Follow>(userConnection);

            if (await _genericRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Filed to follow the user");
        }

    }
}
