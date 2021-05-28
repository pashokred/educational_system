using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace educ_system.Models
{
    public class Query
    {
        public string QueryId { get; set; }

        public string Error { get; set; }

        public int ErrorFlag { get; set; }
        
        public List<string> CourseNames { get; set; }
        public List<string> SubjectNames { get; set; }
        public List<string> TeacherNames { get; set; }
        public List<string> StudentNames { get; set; }
        public List<int> StudentIds { get; set; }
        public List<decimal> Prices { get; set; }
        
        [Required(ErrorMessage = "Поле необхідно заповнити")]
        public string CourseName { get; set; }
        public string TypeName { get; set; }
        public string SubjectName { get; set; }
        public string StudentName { get; set; }
        [Display(Name = "Кількість учнів X")]
        public int Cnt { get; set; }
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [Range(0, 999999)]
        [Display(Name = "Вартість X")]
        public decimal Price { get; set; }
    }
}