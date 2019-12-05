using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBuddy.Api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeBuddy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutcomeController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OutcomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET api/outcomes
        [HttpGet]
        public async Task<IActionResult> GetOutcome()
        {
            var result = await _dataContext.Outcomes.ToListAsync();

            return Ok(result);
        }

        // GET api/outcomes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOutcome(int id)
        {
            var result = await _dataContext.Outcomes.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/outcomes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/outcomes/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}