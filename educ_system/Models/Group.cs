using System.Collections.Generic;

#nullable disable

namespace educ_system.Models
{
    public partial class Group
    {
        public Group()
        {
            StudentGroups = new HashSet<StudentGroup>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TutorId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Tutor Tutor { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
}
