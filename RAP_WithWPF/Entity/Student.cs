using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WithWPF.Entity
{
    public class Student : Researcher
    {
        public Researcher Supervisor { get; set; }
        public string Degree { get; set; }
    }
}
