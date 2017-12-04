using ProgramStarter.ViewModels;
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
using System.Windows.Shapes;

namespace ProgramStarter.Views
{
    /// <summary>
    /// Interaction logic for StartProgramsWindow.xaml
    /// </summary>
    public partial class StartProgramsWindow : Window
    {
        
        public StartProgramsWindow()
        {            
            InitializeComponent();        
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
        }
    }
}
