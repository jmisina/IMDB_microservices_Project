using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;

namespace UsersAPI.Controllers
{
    [Route("userauth")]
    [ApiController]
    public class UserAuthorisationsController : ControllerBase
    {
        private readonly DataContext _context;

        public UserAuthorisationsController(DataContext context)
        {
            _context = context;
        }

        // GET: userauth
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAuthorisation>>> GetUserAuthorisations()
        {
            return await _context.UserAuthorisations.ToListAsync();
        }

        // GET: userauth/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAuthorisation>> GetUserAuthorisation(int id)
        {
            var userAuthorisation = await _context.UserAuthorisations.FindAsync(id);

            if (userAuthorisation == null)
            {
                return NotFound();
            }

            return userAuthorisation;
        }

        // PUT: userauth/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAuthorisation(int id, UserAuthorisation userAuthorisation)
        {
            if (id != userAuthorisation.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userAuthorisation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAuthorisationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: userauth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAuthorisation>> PostUserAuthorisation(UserAuthorisation userAuthorisation)
        {
            _context.UserAuthorisations.Add(userAuthorisation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserAuthorisationExists(userAuthorisation.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserAuthorisation", new { id = userAuthorisation.UserId }, userAuthorisation);
        }

        // DELETE: userauth/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAuthorisation(int id)
        {
            var userAuthorisation = await _context.UserAuthorisations.FindAsync(id);
            if (userAuthorisation == null)
            {
                return NotFound();
            }

            _context.UserAuthorisations.Remove(userAuthorisation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserAuthorisationExists(int id)
        {
            return _context.UserAuthorisations.Any(e => e.UserId == id);
        }
    }
}
