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
    public class ResearcherController
    {
        private LinkedList<Researcher> researchers;
        public ObservableCollection<Researcher> viewableResearchers { get; set; }
        //public ObservableCollection<Researcher> visibleResearchers { get { return viewableResearchers; } set { } }

        public ResearcherController()
        {
            LoadResearchers();
        }

        public void LoadResearchers()
        {
            ResearcherAdapter adapter = new ResearcherAdapter();
            researchers = adapter.fetchBasicResearcherDetails();
            viewableResearchers = new ObservableCollection<Researcher>(researchers);
        }

        public void Reload()
        {
            ResearcherAdapter adapter = new ResearcherAdapter();
            researchers = adapter.fetchBasicResearcherDetails();
            viewableResearchers.Clear();
            foreach(Researcher r in researchers)
            {
                viewableResearchers.Add(r);
            }
            //GetViewableList();
        }

        //public ObservableCollection<Researcher> GetViewableList()
        //{
        //    return visibleResearchers;
        //}

        public void FilterByLevel(EmploymentLevel level)
        {
            var filtered = from Researcher r in researchers
                           where r.Level == level
                           select r;
            researchers = new LinkedList<Researcher>(filtered);
            viewableResearchers.Clear();
            foreach(Researcher r in researchers)
            {
                viewableResearchers.Add(r);
            }
            //GetViewableList();
        }

        public void FilterByName(string name)
        {
            var filtered = from Researcher r in researchers
                           where r.FullName.Contains(name)
                           select r;
            researchers = new LinkedList<Researcher>(filtered);
            viewableResearchers.Clear();
            foreach (Researcher r in researchers)
            {
                viewableResearchers.Add(r);
            }
            //GetViewableList();
        }

        public Researcher GetResearcherByName(string fullname)
        {
            var selected = from Researcher r in researchers
                           where String.Equals(r.FullName, fullname)
                           select r;
            return (Researcher)selected.First();
        }

        public void PrintList()
        {
            foreach (Researcher r in researchers)
            {
                Console.WriteLine("Id: " + r.id + " Full Name: " + r.FullName + " Title: " + r.Title + " Level: " + r.Level);
            }
        }

        public void PrintResearcher(Researcher r)
        {
            Console.WriteLine("Id: " + r.id + " Full Name: " + r.FullName + " Title: " + r.Title + " Unit: " +
                    r.Unit + " Campus: " + r.Campus + " Email: " + r.Email + " Level: " + r.Level + " institution start date:" + r.institutionStartTime);

            if (r is Staff)
            {
                Console.WriteLine("Current Position: " + r.position.Title());
                Console.WriteLine("Prev Position: ");
                foreach (Position p in r.PrevPosition)
                {
                    Console.WriteLine("JobTitle: " + p.Title() + " Start: " + p.start + " End: " + p.end);
                }

                Console.Write("Supervisions: ");
                foreach (Researcher s in ((Staff)r).Supervisions)
                {
                    Console.Write(s.FullName + " ");
                }
            }
            if (r is Student)
            {
                Console.WriteLine("Supervisor: " + ((Student)r).Supervisor.FullName);
            }
        }
    }
}
