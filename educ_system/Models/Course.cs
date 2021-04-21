using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;


#nullable disable

namespace educ_system.Models
{
    public partial class Course
    {
        public Course()
        {
            Groups = new HashSet<Group>();
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        [DisplayName("Teacher")]
        public int TeacherId { get; set; }
        [Required(ErrorMessage = "The field must not be empty")]
        [Range(0, 999999)]
        public decimal? Price { get; set; }
        public int TypeId { get; set; }
        [Required(ErrorMessage = "The field must not be empty")]
        [RegularExpression(@"^[a-zA-Z]+.*$", ErrorMessage = "First alphabet and unique")]
        public string Name { get; set; }
        public string Info { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
