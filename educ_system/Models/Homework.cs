using System.Collections.Generic;

#nullable disable

namespace educ_system.Models
{
    public partial class Homework
    {
        public Homework()
        {
            Marks = new HashSet<Mark>();
        }

        public int Id { get; set; }
        public string Task { get; set; }
        public int LessonId { get; set; }

        public virtual Lesson IdNavigation { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
    }
}
