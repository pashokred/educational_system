using System.Linq;
using System.Threading.Tasks;
using educ_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Type = educ_system.Models;

namespace educ_system.Controllers
{
    public class CoursesController : Controller
    {
        private readonly DBOSchoolContext _context;

        public CoursesController(DBOSchoolContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int? id, string? name, string? info)
        {
            if (id == null) return RedirectToAction("Index", "Types");

            ViewBag.TypeId = id;
            ViewBag.TypeName = name;
            ViewBag.TypeInfo = info;

            var coursesByType = _context.Courses.Where(c => c.TypeId == id).Include(c => c.Type).Include(c => c.Teacher).Include(c => c.Teacher.Subject);

            return View(await coursesByType.ToListAsync());
        }

        public async Task<IActionResult> TypeCourses(int? id)
        {
            return RedirectToAction("Details", "Types", new { id = id });
        }

        public async Task<IActionResult> Types()
        {
            return RedirectToAction("Index", "Types", new { });
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Type)
                .Include(c => c.Teacher.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public async Task<IActionResult> Teacher(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id);
            if (@type == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Teachers", new { id = @type.Id });
        }

        public async Task<IActionResult> Subject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id);
            if (@type == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Subjects", new { id = @type.SubjectId });
        }

        // GET: Courses/Create
        public IActionResult Create(int typeId)
        {
            // ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name");
            ViewBag.TypeId = typeId;
            ViewBag.TeacherNames = new SelectList(_context.Teachers, "Id", "Name");
            ViewBag.TypeInfo = _context.Types.FirstOrDefault(t => t.Id == typeId)?.Info;
            ViewBag.TypeName = _context.Types.FirstOrDefault(t => t.Id == typeId)?.Name;
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int typeId, [Bind("Id,TeacherId,Price,TypeId,Name,Info")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Courses", new
                {
                    id = typeId,
                    name = _context.Types.FirstOrDefault(t => t.Id == typeId)?.Name,
                    info = _context.Types.FirstOrDefault(t => t.Id == typeId)?.Info
                });
            }
            // ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", course.TeacherId);
            // ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", course.TypeId);
            // return View(course);

            return RedirectToAction("Index", "Courses", new
            {
                id = typeId,
                name = _context.Types.FirstOrDefault(t => t.Id == typeId)?.Name,
                info = _context.Types.FirstOrDefault(t => t.Id == typeId)?.Info
            });
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", course.TeacherId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", course.TypeId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherId,Price,TypeId,Name,Info")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Courses", new
                    {
                        id = course.TypeId,
                        name = _context.Types.FirstOrDefault(t => t.Id == course.TypeId)?.Name,
                        info = _context.Types.FirstOrDefault(t => t.Id == course.TypeId)?.Info
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            //ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", course.TeacherId);
            //ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", course.TypeId);
            //return View(course);
            return RedirectToAction("Index", "Courses", new
            {
                id = course.TypeId,
                name = _context.Types.FirstOrDefault(t => t.Id == course.TypeId)?.Name,
                info = _context.Types.FirstOrDefault(t => t.Id == course.TypeId)?.Info
            });
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Courses", new
            {
                id = course.TypeId,
                name = _context.Types.FirstOrDefault(t => t.Id == course.TypeId)?.Name,
                info = _context.Types.FirstOrDefault(t => t.Id == course.TypeId)?.Info
            });
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
