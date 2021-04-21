#nullable disable

namespace educ_system.Models
{
    public partial class Lesson
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Theme { get; set; }
        public string Link { get; set; }
        public string Material { get; set; }

        public virtual Course Course { get; set; }
        public virtual Homework Homework { get; set; }
    }
}
