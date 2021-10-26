using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RAP_WithWPF.Controller;
using RAP_WithWPF.Entity;
using RAP_WithWPF.Database;
using System.Collections.ObjectModel;

namespace RAP_WithWPF.View
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : UserControl
    {
        private ResearcherController rcontroller;
        public static Researcher selected;
        private const string RESEARCHER_LIST_KEY = "researcherList";

        public DetailsView()
        {
            InitializeComponent();
            
            rcontroller = (ResearcherController)(Application.Current.FindResource(RESEARCHER_LIST_KEY) as ObjectDataProvider).ObjectInstance;
            ResearcherList.SelectResearcher += DetailsFillin;
        }

        public static void SetSelected(Researcher r)
        {
            selected = r;
            //Console.WriteLine(selected.CurrentJobTitle());
            
        }

        public void clean()
        {
            //this.fullname.Content = "";
            //this.title.Content = "";
            //this.unit.Content = "";
            //this.campus.Content = "";
            //this.email.Content = "";
            this.position.Content = "";
            //this.institutionStartDate.Content = "";
            this.positionStartDate.Content = "";
            this.PrePosition.ItemsSource = null;
            this.Tenure.Content = "";
            this.publicationsCount.Content = "";
            this.ThreeyearAverage.Content = "";
            this.performance.Content = "";
            this.supervisions.Content = "";
            this.degree.Content = "";
            this.supervisor.Content = "";
            this.avatar.Source = null;
        }

        public void DetailsFillin(object sender, Researcher r)
        {
            clean();

            this.avatar.Source = new BitmapImage(r.Photo);
            this.Tenure.Content = r.Tenure.ToString();

            PublicationAdapter pAdapter = new PublicationAdapter();
            pAdapter.fetchBasicPublicationDetails(r);

            this.ThreeyearAverage.Content = r.ThreeYearAverage;
            this.performance.Content = r.Performance;
            Console.WriteLine(r.Performance);
            this.publicationsCount.Content = r.PublicationsCount();
            Console.WriteLine(r.PublicationsCount());
            if(r is Staff)
            {
                this.position.Content = r.CurrentJobTitle();
                this.positionStartDate.Content = r.position.start;
                this.positionStartDate.ContentStringFormat = "{}{0:dd/MM/yyyy}";
                this.supervisions.Content = ((Staff)r).Supervisions.Count();
                Console.WriteLine(((Staff)r).Supervisions.Count());
                this.PrePosition.ItemsSource = r.PrevPosition;
                
            }
            else
            {
                this.position.Content = "Student";
                this.supervisor.Content = ((Student)r).Supervisor.FullName;
                Console.WriteLine(((Student)r).Supervisor.FullName);
                this.degree.Content = ((Student)r).Degree;
            }
            
            
        }

        private void ShowNamesBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Researcher> supervision = new ObservableCollection<Researcher>(((Staff)selected).Supervisions);
            ShowNameWindow win = new ShowNameWindow();
            win.supervisionList.ItemsSource = supervision;
            win.Show();
        }

        private void CumulativeBtn_Click(object sender, RoutedEventArgs e)
        {
            PublicationsController pcontroller = new PublicationsController();
            pcontroller.LoadPublicationsFor(selected);
            LinkedList<Publication_Count> tmp = pcontroller.CumulativeCount();
            ObservableCollection<Publication_Count> cumulative = new ObservableCollection<Publication_Count>(tmp);
            CumulativeWindow cwin = new CumulativeWindow();
            cwin.cumulativeListBox.ItemsSource = cumulative;
            cwin.Show();
        }

    }
}
