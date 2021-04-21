using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using educ_system.Models;

namespace educ_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DBOSchoolContext _context;
        public ChartsController(DBOSchoolContext context)
        {
            _context = context;
        }

        [HttpGet("JsonDataTypes")]
        public JsonResult JsonDataTypes()
        {
            var types = _context.Types.Include(c => c.Courses).ToList();
            var typeCourse = new List<object> {new[] {"Type", "Amount of courses"}};
            typeCourse.AddRange(types.Select(t => new object[] {t.Name, t.Courses.Count }));
            return new JsonResult(typeCourse);
        }

        [HttpGet("JsonDataTeachers")]
        public JsonResult JsonDataTeachers()
        {
            var teachers = _context.Teachers.Include(t => t.Courses).ToList();
            var courseTeacher = new List<object> {new[] {"Course", "Amount of teachers"}};
            courseTeacher.AddRange(teachers.Select(t => new object[] {t.Name, t.Courses.Count}));
            return new JsonResult(courseTeacher);
        }
    }
}
