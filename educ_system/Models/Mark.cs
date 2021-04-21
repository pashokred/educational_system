using System;
using System.Collections.Generic;
using educ_system.Models;

#nullable disable

namespace educ_system
{
    public partial class Mark
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HomeworkId { get; set; }
        public int Mark1 { get; set; }

        public virtual Homework Homework { get; set; }
        public virtual Student Student { get; set; }
    }
}
