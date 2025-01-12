using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.DTO;
using UsersAPI.Models;

namespace UsersAPI.Controllers
{
    [Route("addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly DataContext _context;

        public AddressesController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<GetUserAddressResult>> GetUsers()
        {
            var result = await (from u in _context.Users
                                join a in _context.Addresses on u.Id equals a.UserId
                                select new GetUserAddressResult
                                {
                                    UserId = u.Id,
                                    AddressType = a.AddressType,
                                    AddressLine = a.AddressLine,
                                    City = a.City,
                                    PostalCode = a.PostalCode,
                                    Country = a.Country,
                                })
                                .ToListAsync(); ;


            return result;
        }

        // GET: /addresses/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<GetUserAddressResult> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var userAddress = await _context.Addresses.FindAsync(id);

            if (user == null | userAddress == null)
            {
                throw new Exception("User not found!");
            }

            var response = new GetUserAddressResult()
            {
                UserId = user.Id,
                AddressType = userAddress.AddressType,
                AddressLine = userAddress.AddressLine,
                City = userAddress.City,
                PostalCode = userAddress.PostalCode,
                Country = userAddress.Country,

            };
            return response;
        }

        // PUT: /adresses/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, ModifyAddressCommand command)
        {
            var currentDateTime = DateTime.UtcNow;
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                var address = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == id);

                if (address != null)
                {
                    address.AddressLine = command.AddressLine;
                    address.City = command.City;
                    address.PostalCode = command.PostalCode;
                    address.Country = command.Country;
                    user.UpdatedAt = currentDateTime;

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return NotFound(new { Message = "Address not found." });
                }
            }
            else
            {
                return NotFound(new { Message = "User not found." });
            }
        }
    }
}
