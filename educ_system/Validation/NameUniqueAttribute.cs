using System.ComponentModel.DataAnnotations;
using System.Linq;
using educ_system.Models;

namespace educ_system.Validation
{
    public class TeacherNameUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (DBOSchoolContext) validationContext.GetService(typeof(DBOSchoolContext));
            var entity = context.Teachers.SingleOrDefault(t => t.Name == value.ToString());

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string name)
        {
            return $"Name {name} is already in use.";
        }
    }
}