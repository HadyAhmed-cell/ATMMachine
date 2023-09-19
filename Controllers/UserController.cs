using ATMMachine.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATMMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("verify")]
        public async Task<ActionResult> UserVerification(int pin, long creditno)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest();
            }

            var user = await _context.Users.Where(x => x.Pin == pin && x.CreditCardNo == creditno).SingleOrDefaultAsync();

            if ( user == null )
            {
                return Unauthorized("Invalid Credentials !");
            }
            else
            {
                return Ok(new
                {
                    Message = "User Verified",
                    UserId = user.Id,
                });
            }
        }

        [HttpGet("BalanceInquiry")]
        public async Task<ActionResult> BalanceInquiry(int id)
        {
            var user = await _context.Users.Where(x => x.Id == id).SingleOrDefaultAsync();

            return Ok(user.AccountBalance);
        }

        [HttpPut("CashWithdrawl")]
        public async Task<ActionResult> CashWithdrawl(int id, double amount)
        {
            if ( amount > 1000 )
            {
                return BadRequest("CashWithdrawl Can't Exceed 1000 L.E Per Transaction");
            }
            var user = await _context.Users.Where(x => x.Id == id).SingleOrDefaultAsync();

            user.AccountBalance -= amount;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return Ok("Transaction Completed");
        }
    }
}