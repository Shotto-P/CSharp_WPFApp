using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using RAP_WithWPF.Controller;
using RAP_WithWPF.Database;
using RAP_WithWPF.Entity;

namespace RAP_WithWPF.View
{
    /// <summary>
    /// Interaction logic for ShowNameWindow.xaml
    /// </summary>
    public partial class ShowNameWindow : Window
    {
        private ResearcherController rcontroller;
        private const string RESEARCHER_LIST_KEY = "researcherList";
        public ShowNameWindow()
        {
            InitializeComponent();

            rcontroller = (ResearcherController)(Application.Current.FindResource(RESEARCHER_LIST_KEY) as ObjectDataProvider).ObjectInstance;
        }
    }
}
