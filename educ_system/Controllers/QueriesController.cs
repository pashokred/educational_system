using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using educ_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using educ_system.Models;

namespace educ_system.Controllers
{
    public class QueriesController : Controller
    {
        private const string ConnStr = "Server=ADMIN-PASHOK;Database=DBOSchool;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static readonly string BaseDirQueries = @"D:\Development\C#\educ_system\educ_system\Queries\";


        private static readonly string S1Path = BaseDirQueries + "Simple1.sql";
        private static readonly string S2Path = BaseDirQueries + "Simple2.sql";
        private static readonly string S3Path = BaseDirQueries + "Simple3.sql";
        private static readonly string S4Path = BaseDirQueries + "Simple4.sql";
        private static readonly string S5Path = BaseDirQueries + "Simple5.sql";
        private static readonly string S6Path = BaseDirQueries + "Simple6.sql";
        private static readonly string A1Path = BaseDirQueries + "Advanced1.sql";
        private static readonly string A2Path = BaseDirQueries + "Advanced2.sql";
        private static readonly string A3Path = BaseDirQueries + "Advanced34.sql";
        private static readonly string T1Path = BaseDirQueries + "Teacher1.sql";
        private static readonly string T2Path = BaseDirQueries + "Teacher2.sql";

        private const string ERR_AVG = "Неможливо обрахувати середню ціну, оскільки продукти відсутні.";
        private const string ERR_CUST = "Покупці, що задовольняють дану умову, відсутні.";
        private const string ERR_PROD = "Програмні продукти, що задовольняють дану умову, відсутні.";
        private const string ERR_DEV = "Розробники, що задовольняють дану умову, відсутні.";
        private const string ERR_COUNTRY = "Країни, що задовольняють дану умову, відсутні.";
        
        private readonly DBOSchoolContext _context;

        public QueriesController(DBOSchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index(int errorCode)
        {
            if (errorCode == 1)
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.PriceError = "Введіть коректну вартість";
            }
            if (errorCode == 2)
            {
                ViewBag.ErrorFlag = 2;
                ViewBag.ProdNameError = "Поле необхідно заповнити";
            }

            var empty = new SelectList(new List<string> { "--Пусто--" });
            var anyTypes = _context.Types.Any();
            var anyCourses = _context.Courses.Any();
            var anySubjects = _context.Subjects.Any();
            var anyStudents = _context.Students.Any();
            var anyTeachers = _context.Teachers.Any();
            
            ViewBag.CourseNames = anyCourses ? new SelectList(_context.Courses, "Name", "Name") : empty;
            ViewBag.TypeNames = anyTypes ? new SelectList(_context.Types, "Name", "Name") : empty;
            ViewBag.SubjectNames = anySubjects ? new SelectList(_context.Subjects, "Name", "Name") : empty;
            ViewBag.StudentIds = anyStudents ? new SelectList(_context.Students, "Id", "Id") : empty;
            ViewBag.StudentNames = anyStudents ? new SelectList(_context.Students, "Name", "Name") : empty;
            ViewBag.TeacherIds = anyTeachers ? new SelectList(_context.Teachers, "Id", "Id") : empty;
            ViewBag.TeacherNames = anyTeachers ? new SelectList(_context.Teachers, "Name", "Name") : empty;
            ViewBag.Prices = anyCourses ? new SelectList(_context.Courses, "Price", "Price") : empty;
            ViewBag.StudentIds = anyStudents ? new SelectList(_context.Students, "Id", "Id") : empty;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S1Path);
            query = query.Replace("X", "N\'" + queryModel.CourseName + "\'");
            query = query.Replace("Y", "N\'" + queryModel.TypeName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S1";
            queryModel.StudentNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.StudentNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S2Path);
            query = query.Replace("X", "N\'" + queryModel.TypeName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S2";
            queryModel.StudentNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.StudentNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S3Path);
            query = query.Replace("X", "N\'" + queryModel.SubjectName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S3";
            queryModel.CourseNames = new List<string>();
            queryModel.Prices = new List<decimal>();
            
            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.CourseNames.Add(reader.GetString(0));
                            queryModel.Prices.Add(reader.GetDecimal(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery4(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S4Path);
            query = query.Replace("X", "N\'" + queryModel.StudentName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S4";
            queryModel.TeacherNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.TeacherNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery5(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S5Path);
            query = query.Replace("X", "N\'" + queryModel.Price + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S5";
            queryModel.TeacherNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.TeacherNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery6(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S6Path);
            query = query.Replace("X", "N\'" + queryModel.SubjectName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "S6";
            queryModel.StudentNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.StudentNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A1Path);
            query = query.Replace("Y", queryModel.TeacherId.ToString());
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "A1";
            queryModel.SubjectNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.SubjectNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A2Path);
            query = query.Replace("Y", "N\'" + queryModel.StudentName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "A2";
            queryModel.StudentNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.StudentNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        public IActionResult AdvancedQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A3Path);
            query = query.Replace("Y", "N\'" + queryModel.StudentName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "A3";
            queryModel.StudentIds = new List<int>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.StudentIds.Add(reader.GetInt32(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        public IActionResult TeacherQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(T1Path);
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "T1";
            queryModel.SubjectNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.SubjectNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        public IActionResult TeacherQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(T2Path);
            query = query.Replace("X", "N\'" + queryModel.Cnt + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "T2";
            queryModel.TeacherNames = new List<string>();

            using (var connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.TeacherNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        public IActionResult Result(Query queryResult)
        {
            return View(queryResult);
        }
    }
}