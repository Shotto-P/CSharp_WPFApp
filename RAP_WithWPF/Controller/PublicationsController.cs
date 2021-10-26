using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WithWPF.Entity;
using RAP_WithWPF.Database;
using System.Collections.ObjectModel;

namespace RAP_WithWPF.Controller
{
    public class PublicationsController
    {
        public LinkedList<Publication> publications;
        public ObservableCollection<Publication> viewablePublication { get; set; }

        public void LoadPublicationsFor(Researcher r)
        {
            PublicationAdapter adapter = new PublicationAdapter();
            publications = adapter.fetchBasicPublicationDetails(r);
            viewablePublication = new ObservableCollection<Publication>(publications);
        }

        public Publication GetPublicationByTitle(string title)
        {
            var selected = from Publication p in publications
                           where String.Equals(p.Title, title)
                           select p;
            return (Publication)selected.First();
        }

        public void FilterByYearRange(int? start, int? end)
        {
            var filtered = from Publication p in publications
                           where p.Year < (end+1) && p.Year > (start-1)
                           select p;
            publications = new LinkedList<Publication>(filtered);

            viewablePublication.Clear();
            foreach(Publication p in publications)
            {
                viewablePublication.Add(p);
            }
        }

        public LinkedList<Publication_Count> CumulativeCount()
        {
            //first iterate the publication list to filter out how many different year it contains
            LinkedList<Int32> years = new LinkedList<Int32>();
            foreach(Publication p in publications)
            {
                if (!years.Contains(p.Year))
                {
                    years.AddLast(p.Year);
                }
            }

            //then, count each year has how many items of it.
            LinkedList<Publication_Count> pc = new LinkedList<Publication_Count>();
            foreach(int year in years)
            {
                var selected = from Publication p in publications
                               where p.Year == year
                               select p;
                Publication_Count count = new Publication_Count
                {
                    Year = year,
                    Count = selected.Count()
                };
                pc.AddLast(count);
            }

            LinkedList<Publication_Count> cumulativeList = new LinkedList<Publication_Count>(pc.OrderBy(p => p.Year));
            return cumulativeList;
        }
        public void PrintPublicationList()
        {
            foreach (Publication p in publications)
            {
                Console.WriteLine("Title: " + p.Title + " Authors: " + p.Authors + " Type: " + p.Type.ToString() + " Year: " + p.Year);
            }
        }

        public void PrintPublication(Publication p)
        {
            Console.WriteLine("DOI: " + p.DOI);
            Console.WriteLine("Title: " + p.Title);
            Console.WriteLine("Authors: " + p.Authors);
            Console.WriteLine("Year: " + p.Year);
            Console.WriteLine("Type: " + p.Type.ToString());
            Console.WriteLine("Cite As: " + p.CitedAs);
            Console.WriteLine("Available: " + p.Available);
            Console.WriteLine("Age: " + p.Age() + " days");
        }

        public void SortByAscending()
        {
            this.publications = new LinkedList<Publication>(this.publications.OrderBy(p => p.Year));
            viewablePublication.Clear();
            foreach (Publication p in publications)
            {
                viewablePublication.Add(p);
            }
        }

        public void SortByDescending()
        {
            this.publications = new LinkedList<Publication>(this.publications.OrderByDescending(p => p.Year));
            viewablePublication.Clear();
            foreach (Publication p in publications)
            {
                viewablePublication.Add(p);
            }
        }
    }
}
