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
using RAP_WithWPF.Entity;
using RAP_WithWPF.Controller;
using RAP_WithWPF.Database;

namespace RAP_WithWPF.View
{
    /// <summary>
    /// Interaction logic for PublicationView.xaml
    /// </summary>
    public partial class PublicationView : UserControl
    {
        private PublicationsController pcontroller;
        public Researcher researcher = null;
        public PublicationView()
        {
            InitializeComponent();

            pcontroller = new PublicationsController();
            ResearcherList.SelectResearcher += FillPublicationList;
        }

        public void clean()
        {
            //this.publications.ItemsSource = null;
            this.doi.Content = "";
            this.title.Text = "";
            this.authors.Content = "";
            this.type.Content = "";
            this.year.Content = "";
            this.citeAs.Text = "";
            this.available.Content = "";
            this.age.Content = "";
        }

        public void FillPublicationList(object sender, Researcher r)
        {
            clean();
            Console.WriteLine("test");
            pcontroller.LoadPublicationsFor(r);
            this.publications.ItemsSource = pcontroller.viewablePublication;
        }

        private void publications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                Publication selected = (Publication)this.publications.SelectedItem;
                PublicationAdapter pAdapter = new PublicationAdapter();
                selected = pAdapter.completePublicationDetails(selected);
                FillPublicationDetails(selected);
            }
        }

        public void FillPublicationDetails(Publication p)
        {
            this.detailsGrid.DataContext = p;
            this.doi.Content = p.DOI;
            this.title.Text = p.Title;
            this.authors.Content = p.Authors;
            this.type.Content = p.Type;
            this.year.Content = p.Year;
            this.citeAs.Text = p.CitedAs;
            this.available.Content = p.Available;
            this.age.Content = p.BookAge;
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            int? start = null;
            int? end = null;
            string startYear = this.startYearTextBox.Text;
            string endYear = this.endYearTextBox.Text;
            if(!String.Equals(startYear, ""))
            {
                start = Int32.Parse(startYear);
            }
            if(!String.Equals(endYear, ""))
            {
                end = Int32.Parse(endYear);
            }
            
            string order = ((ComboBoxItem)this.orderComboBox.SelectedItem).Content.ToString();
            if((start != null) && (end != null))
            {
                if(start < end)
                {
                    pcontroller.FilterByYearRange(start, end);
                }
                else
                {
                    MessageBox.Show("Please ensure startYear is before endYear....");
                }
            }
            if(((start != null)&& (end == null)) || ((start == null) && (end != null)))
            {
                MessageBox.Show("Please enter both startYear and endYear.....");
            }
            
            switch (order)
            {
                case "Ascending":
                    pcontroller.SortByAscending();
                    break;
                case "Descending":
                    pcontroller.SortByDescending();
                    break;
                case "No Order":
                    break;
            }

            //clean the input text box after execute the command
            this.startYearTextBox.Text = "";
            this.endYearTextBox.Text = "";
        }
    }
}
