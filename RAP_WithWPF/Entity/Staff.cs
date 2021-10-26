using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WithWPF.Entity
{
    public class Staff : Researcher
    {
        public LinkedList<Researcher> Supervisions { get; set; }
    }
}
