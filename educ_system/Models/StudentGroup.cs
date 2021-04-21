#nullable disable

namespace educ_system.Models
{
    public partial class StudentGroup
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Student Student { get; set; }
    }
}
