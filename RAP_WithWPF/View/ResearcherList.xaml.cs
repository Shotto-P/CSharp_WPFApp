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

namespace RAP_WithWPF.View
{
    /// <summary>
    /// Interaction logic for ResearcherList.xaml
    /// </summary>
    public delegate void ResearcherEventHandler(object sender, Researcher r);
    public partial class ResearcherList : UserControl
    {
        public static event ResearcherEventHandler SelectResearcher;
        private const string RESEARCHER_LIST_KEY = "researcherList";
        private ResearcherController rcontroller;
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Researcher), typeof(ResearcherList));
        public Researcher SelectedItem
        {
            get { return (Researcher)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public ResearcherList()
        {
            InitializeComponent();
            rcontroller = (ResearcherController)(Application.Current.FindResource(RESEARCHER_LIST_KEY) as ObjectDataProvider).ObjectInstance;
            
            rlist.ItemsSource = rcontroller.viewableResearchers;
            rlist.SelectionChanged += ListBox_SelectionChanged;
            rlist.SelectedIndex = -1;
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                //Researcher r = (Researcher)e.AddedItems[0];
                Researcher temp = (Researcher)rlist.SelectedItem;
                ResearcherAdapter rAdapter = new ResearcherAdapter();
                temp = rAdapter.fetchFullResearcherDetails(temp.id);
                temp = rAdapter.completeResearcherDetails(temp);
                SelectedItem = temp;

                ResearcherEventHandler handler = SelectResearcher;
                handler?.Invoke(this, SelectedItem);

                DetailsView.SetSelected(SelectedItem);
            }
        }
    }
}
