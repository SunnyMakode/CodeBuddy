using System.Threading.Tasks;
using CodeBuddy.Api.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeBuddy.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OutcomeController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OutcomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET api/outcome
        [HttpGet]
        public async Task<IActionResult> GetOutcome()
        {
            var result = await _dataContext.Outcomes.ToListAsync();

            return Ok(result);
        }

        // GET api/outcome/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOutcome(int id)
        {
            var result = await _dataContext.Outcomes.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(result);
        }

        // POST api/outcome
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/outcome/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/outcome/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}