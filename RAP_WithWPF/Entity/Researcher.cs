using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WithWPF.Entity
{
    public class Researcher
    {
        public int id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
        public EmploymentLevel Level { get; set; }
        public Uri Photo { get; set; }
        public Position position { get; set; }
        public LinkedList<Position> PrevPosition { get; set; }
        public LinkedList<Publication> publications { get; set; }
        public DateTime institutionStartTime { get; set; }
        public double Tenure { get; set; }
        public double ThreeYearAverage
        {
            get
            {
                return Math.Round((this.PublicationsCount() / 3d), 1, MidpointRounding.ToEven);
            }
            set { }
        }
        public string Performance
        {
            get
            {
                switch (this.Level)
                {
                    case EmploymentLevel.A:
                        return (this.ThreeYearAverage / 0.5).ToString("P", CultureInfo.InvariantCulture);
                    case EmploymentLevel.B:
                        return (this.ThreeYearAverage / 1d).ToString("P", CultureInfo.InvariantCulture);
                    case EmploymentLevel.C:
                        return (this.ThreeYearAverage / 2d).ToString("P", CultureInfo.InvariantCulture);
                    case EmploymentLevel.D:
                        return (this.ThreeYearAverage / 3.2).ToString("P", CultureInfo.InvariantCulture);
                    case EmploymentLevel.E:
                        return (this.ThreeYearAverage / 4d).ToString("P", CultureInfo.InvariantCulture);
                    case EmploymentLevel.Student:
                        return "";
                    default:
                        return "";
                }
            }
            set { }
        }

        public Position GetCurrentJob()
        {
            return this.position;
        }

        public string CurrentJobTitle()
        {
            return this.position.Title();
        }

        public DateTime CurrentJobStart()
        {
            return this.position.start;
        }

        public Position GetEarliestJob()
        {
            return this.PrevPosition.Last();
        }

        public DateTime EarliestStart()
        {
            return this.GetEarliestJob().start;
        }

        public int PublicationsCount()
        {
            return publications.Count();
        }
    }
}
