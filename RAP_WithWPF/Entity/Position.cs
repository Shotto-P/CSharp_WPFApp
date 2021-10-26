using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WithWPF.Entity
{
    public enum EmploymentLevel { Student, A, B, C, D, E };
    public class Position
    {
        public EmploymentLevel level { get; set; }
        public DateTime start { get; set; }
        public DateTime? end { get; set; }
        public string JobTitle { get { return this.Title(); } set { } }

        public string Title()
        {
            if (String.Equals(this.ToTitle(this.level), "Student"))
            {
                return "Student";
            }
            else if (String.Equals(this.ToTitle(this.level), "A"))
            {
                return "Postdoc";
            }
            else if (String.Equals(this.ToTitle(this.level), "B"))
            {
                return "Lecturer";
            }
            else if (String.Equals(this.ToTitle(this.level), "C"))
            {
                return "Senior Lecturer";
            }
            else if (String.Equals(this.ToTitle(this.level), "D"))
            {
                return "Associate Professor";
            }
            else if (String.Equals(this.ToTitle(this.level), "E"))
            {
                return "Professor";
            }
            else
            {
                return "No Title";
            }
        }

        public string ToTitle(EmploymentLevel level)
        {
            return level.ToString();
        }
    }
}
