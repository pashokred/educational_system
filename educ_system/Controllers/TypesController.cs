using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using educ_system.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Type = educ_system.Models.Type;

namespace educ_system.Controllers
{
    public class TypesController : Controller
    {
        private readonly DBOSchoolContext _context;

        public TypesController(DBOSchoolContext context)
        {
            _context = context;
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
            return View(await _context.Types.ToListAsync());
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@type == null)
            {
                return NotFound();
            }

            // return View(@type);
            return RedirectToAction("Index", "Courses", new { id = @type.Id, name = @type.Name, info = @type.Info});
        }

        // GET: Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info")] Type @type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@type);
        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }
            return View(@type);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Info")] Type @type)
        {
            if (id != @type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(@type.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@type);
        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @type = await _context.Types.FindAsync(id);
            _context.Types.Remove(@type);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(int id)
        {
            return _context.Types.Any(e => e.Id == id);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
            if (fileExcel != null)
            {
                await using var stream = new FileStream(fileExcel.FileName, FileMode.Create);
                await fileExcel.CopyToAsync(stream);
                using XLWorkbook workbook = new XLWorkbook(stream, XLEventTracking.Disabled);
                
                // check all worksheets (in current case types)
                foreach (IXLWorksheet worksheet in workbook.Worksheets)
                {
                    // worksheet.Name - type name. Try to find in DB, if not exist - create new one
                    Type newType;
                    var t = _context.Types.Where(typ => typ.Name.Contains(worksheet.Name)).ToList();
                    
                    if (t.Count > 0)
                        newType = t.First();
                    else
                    {
                        newType = new Type { Name = worksheet.Name, Info = "from EXCEL" };
                        // add to context
                        _context.Types.Add(newType);
                    }
                    // check all rows                    
                    foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                    {
                        try
                        {
                            var course = new Course
                            {
                                Price = Convert.ToDecimal(row.Cell(1).Value),
                                Name = row.Cell(2).Value.ToString(),
                                Info = row.Cell(3).Value.ToString(),
                                Type = newType,
                            };
                            
                            var teacherName = row.Cell(4).Value.ToString();
                            var subjectName = row.Cell(5).Value.ToString();
                            if (teacherName?.Length <= 0 || subjectName?.Length <= 0) continue;
                            Teacher teacher;
                            Subject subject;
                            
                            // Find subject in case exists, else - add
                            var subjectsWithName = _context.Subjects.Where(sbj => sbj.Name.Contains(subjectName));
                            if (subjectsWithName.Any())
                                subject = subjectsWithName.First();
                            else
                            {
                                subject = new Subject { Name = subjectName };
                                _context.Add(subject);
                            }
                            
                            // Find teacher in case exists, else - add
                            var teachersWithName = _context.Teachers.Where(tr => tr.Name.Contains(teacherName) 
                                                                  && tr.Subject.Name.Contains(subject.Name)).ToList();
                            if (teachersWithName.Count > 0)
                                teacher = teachersWithName.First();
                            else
                            {
                                teacher = new Teacher { Name = teacherName, Info = "from EXCEL", Subject = subject};
                                _context.Add(teacher);
                            }

                            course.Teacher = teacher;
                            _context.Courses.Add(course);
                        }
                        catch (Exception)
                        {
                            // logging
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using var workbook = new XLWorkbook(XLEventTracking.Disabled);
            var types = _context.Types.Include(t => t.Courses);
             //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
             foreach (var t in types)
             {
                 var worksheet = workbook.Worksheets.Add(t.Name);

                 worksheet.Cell("A1").Value = "Price";
                 worksheet.Cell("B1").Value = "Name";
                 worksheet.Cell("C1").Value = "Info";
                 worksheet.Cell("D1").Value = "Teacher";
                 worksheet.Cell("E1").Value = "Subject";
                 worksheet.Row(1).Style.Font.Bold = true;
                 var courses = _context.Courses.Where(c => c.Type == t).Include(c => c.Teacher)
                     .Include(c => c.Teacher.Subject).ToList();

                 //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                 for (var i = 0; i < courses.Count; i++)
                 {
                     worksheet.Cell(i + 2, 1).Value = courses[i].Price;
                     worksheet.Cell(i + 2, 2).Value = courses[i].Name;
                     worksheet.Cell(i + 2, 3).Value = courses[i].Info;
                     worksheet.Cell(i + 2, 4).Value = courses[i].Teacher.Name;
                     worksheet.Cell(i + 2, 5).Value = courses[i].Teacher.Subject.Name;
                 }
             }

             using var stream = new MemoryStream();
             workbook.SaveAs(stream);
             stream.Flush();

             return new FileContentResult(stream.ToArray(),
                 "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
             {
                 FileDownloadName = $"online-school_{DateTime.UtcNow.ToLongDateString()}.xlsx"
             };
        }
    }
}
