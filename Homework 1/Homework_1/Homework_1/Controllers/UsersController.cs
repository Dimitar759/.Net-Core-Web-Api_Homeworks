using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Homework_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<string>> GetAll()
        {

            return Ok(StaticDb.Usernames);
        }

        [HttpGet("index")]
        public ActionResult<List<string>> GetUserName(int index)
        {
            int allUsers = StaticDb.Usernames.Count;

            try
            {
                //validations
                if (index < 0)
                {
                    return BadRequest($"This is a negative number, please enter a number from 0 to {allUsers - 1}");
                }

                if (index >= StaticDb.Usernames.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"There is no user on index {index}, please enter a number from 0 to {allUsers - 1}");
                }

                return StatusCode(StatusCodes.Status200OK, StaticDb.Usernames[index]);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please contact your administrator");
            }
        }
    }
}
