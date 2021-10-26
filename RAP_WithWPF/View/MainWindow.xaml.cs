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

namespace RAP_WithWPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string RESEARCHER_LIST_KEY = "researcherList";
        private ResearcherController rcontroller;
        public MainWindow()
        {
            InitializeComponent();
            rcontroller = (ResearcherController)(Application.Current.FindResource(RESEARCHER_LIST_KEY) as ObjectDataProvider).ObjectInstance;
            
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            //reload the researcherList every time we click the search btn;
            //so as to ensure the search operatioin is operated on the original list
            rcontroller.Reload();

            string name = nameFilterTxtBox.Text;
            string level = ((ComboBoxItem)levelFilterComboBox.SelectedItem).Content.ToString();
            Console.WriteLine(level);
            if(!String.Equals(name, ""))
            {
                rcontroller.FilterByName(name);
            }
            if(!String.Equals(level, ""))
            {
                if(!String.Equals(level, "All Researchers"))
                {
                    rcontroller.FilterByLevel((EmploymentLevel)Enum.Parse(typeof(EmploymentLevel), level));
                    foreach(Researcher r in rcontroller.viewableResearchers)
                    {
                        Console.WriteLine(r.FullName);
                    }

                }
            }

            //clear the input field everytime after the command executed
            this.nameFilterTxtBox.Text = "";
        }
    }
}
