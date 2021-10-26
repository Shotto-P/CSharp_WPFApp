using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WithWPF.Entity
{
    public enum OutputType { Conference, Journal, Other };
    public class Publication
    {
        public string DOI { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public int Year { get; set; }
        public OutputType Type { get; set; }
        public string CitedAs { get; set; }
        public DateTime Available { get; set; }
        public int BookAge { get { return (DateTime.Now - this.Available).Days; } set { } }

        public int Age()
        {
            return (DateTime.Now - this.Available).Days;
        }
    }
}
