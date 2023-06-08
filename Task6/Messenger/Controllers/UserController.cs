using Messenger.Data;
using Messenger.Hubs;
using Messenger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Controller
{
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MessageHub _hub;

        public UserController(AppDbContext context, MessageHub hub)
        {
            _context = context;
            _hub = hub;
        }
   
        [HttpPost]
        public async Task<IActionResult> CheckAndUpdateUserAsync([FromBody] UserDto userDto)
        {
             if (userDto is null)
                return BadRequest();

            var user = userDto.User;
            var connectionId = userDto.ConnectionId;  
            bool userExists = _context.Users.Any(p => p.Name == user);
            if (userExists)
            {
                var someUser = await _context.Users.Where(p => p.Name ==user).FirstOrDefaultAsync();
                someUser.ConnectionId = connectionId;
            }
            else
            {
                await _context.AddAsync(new User{
                    Id = Guid.NewGuid(),
                    Name = user,
                    ConnectionId = connectionId
                });
            }
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet]
        public async Task<IActionResult> FindReceiversAsync()
        {
            string term = HttpContext.Request.Query["search"].ToString();
            var users = _context.Users
                        .Where(p => p.Name.Contains(term.ToLower()))
                        .Select(p => p.Name)
                        .ToListAsync();
            return Ok(await users);
        }
    }
    public class UserDto
    {
        public string User { get; set; }
        public string ConnectionId { get; set; }
    }
}