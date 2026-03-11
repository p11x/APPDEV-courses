/*
================================================================================
TOPIC 33: REST API DEVELOPMENT
================================================================================

REST (Representational State Transfer) APIs are the standard for web services.

TABLE OF CONTENTS:
1. REST Principles
2. HTTP Methods
3. Status Codes
4. API Controllers
5. HTTP Client
================================================================================
*/

namespace RestApiConcepts
{
    // ====================================================================
    // API CONTROLLER EXAMPLE
    // ====================================================================
    
    // Example controller structure:
    /*
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        // GET /api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_userService.GetAll());
        }
        
        // GET /api/users/5
        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        
        // POST /api/users
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            var created = _userService.Create(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        
        // PUT /api/users/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();
            _userService.Update(user);
            return NoContent();
        }
        
        // DELETE /api/users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return NoContent();
        }
    }
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== REST API Concepts ===");
            
            // HTTP Methods
            Console.WriteLine("\nHTTP Methods:");
            Console.WriteLine("GET    - Retrieve data");
            Console.WriteLine("POST   - Create new resource");
            Console.WriteLine("PUT    - Update entire resource");
            Console.WriteLine("PATCH  - Partial update");
            Console.WriteLine("DELETE - Remove resource");
            
            // Status Codes
            Console.WriteLine("\nStatus Codes:");
            Console.WriteLine("200 OK - Success");
            Console.WriteLine("201 Created - Resource created");
            Console.WriteLine("204 No Content - Success, no content to return");
            Console.WriteLine("400 Bad Request - Client error");
            Console.WriteLine("401 Unauthorized - Authentication required");
            Console.WriteLine("404 Not Found - Resource doesn't exist");
            Console.WriteLine("500 Internal Server Error - Server error");
        }
    }
}

/*
REST PRINCIPLES:
----------------
1. Client-Server - Separate concerns
2. Stateless - Each request independent
3. Cacheable - Responses can be cached
4. Uniform Interface - Standard methods
5. Layered System - Can have intermediaries
*/

// ================================================================================
// INTERVIEW QUESTIONS
// =============================================================================

/*
Q1: What is the difference between PUT and PATCH?
A: PUT replaces entire resource, PATCH updates partial fields.

Q2: What does Idempotent mean in REST?
A: Multiple identical requests have same effect as single request.
   GET, PUT, DELETE are idempotent; POST is not.

Q3: What is HATEOAS?
A: Hypermedia as the Engine of Application State - links in responses
   to navigate the API.
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 34 covers Entity Framework Core.
*/
