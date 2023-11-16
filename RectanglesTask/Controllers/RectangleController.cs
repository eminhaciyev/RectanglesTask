using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RectanglesTask.Models;
using RectanglesTask.Models.DAL;

namespace RectanglesTask.Controllers
{
    public class RectangleController : Controller
    {
        private readonly RectangleContext _context;

        public RectangleController(RectangleContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPost("getCoordinates")]
        public IActionResult GetRectanglesForCoordinates([FromBody] List<int[]> coordinates)
        {
            if (coordinates == null || !coordinates.Any())
            {
                return BadRequest("Invalid input.");
            }

            List<Rectangle> result = new List<Rectangle>();

            foreach (var coordinate in coordinates)
            {
                int x = coordinate[0];
                int y = coordinate[1];

                var rectangles = _context.Rectangles
                    .Where(r => x >= r.X && x <= (r.X + r.Width) && y >= r.Y && y <= (r.Y + r.Height))
                    .ToList();

                result.AddRange(rectangles);
            }

            return Ok(result);
        }
    }
}


