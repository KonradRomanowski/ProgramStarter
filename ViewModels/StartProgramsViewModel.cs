using ProgramStarter.Helpers;
using ProgramStarter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProgramStarter.ViewModels
{
    public class StartProgramsViewModel : BaseViewModel
    {
        #region Statics Definition
        public static int StartProgramWindowHeightBig = 300;
        public static int StartProgramWindowHeightSmall = 135;
        #endregion

        #region Variables Definition
        private string mSeconds_To_Start;
        private string mOptionsButtonContent;
        public ICommand StartNowButtonCommand { get; private set; }
        public ICommand DontStartButtonCommand { get; private set; }
        public ICommand OptionsButtonCommand { get; private set; }
        public ICommand ProgramsToStartButtonCommand { get; private set; }               

        public string Seconds_To_Start
        {
            get
            {                
                return mSeconds_To_Start;
            }

            set
            {
                if (mSeconds_To_Start == value)
                    return;

                mSeconds_To_Start = value;
                OnPropertyChanged(nameof(Seconds_To_Start));                                               
            }
        }

        public string OptionsButtonContent
        {
            get
            {
                return mOptionsButtonContent;
            }

            set
            {
                if (mOptionsButtonContent == value)
                    return;

                mOptionsButtonContent = value;
                OnPropertyChanged(nameof(OptionsButtonContent));
            }
        }

        #endregion


        public StartProgramsViewModel()
        {
            OptionsButtonContent = "Options >>>";
            //just for tests - can be deleted            
            //XMLHandler test = new XMLHandler(@"D:\Programy Programowanie\Moje\ProgramStarter\csharp\ProgramStarter\ProgramStarter\Data\configuration.xml");
            //List<ProgramToStart> ttt = test.ReadProgramsToStartList();

            //foreach (var item in ttt)
            //{
            //    MessageBoxResult result = MessageBox.Show(item.StartingOrder.ToString() + ' ' + item.ProgramName + ' ' + item.Path);
            //}
            //------

            //Binding for buttons
            StartNowButtonCommand = new RelayCommand(StartNowButtonClicked);
            DontStartButtonCommand = new RelayCommand(DontStartButtonClicked);
            OptionsButtonCommand = new RelayCommand(OptionsButtonClicked);
            ProgramsToStartButtonCommand = new RelayCommand(ProgramsToStartButtonClicked);

        }

        private void ProgramsToStartButtonClicked(object obj)
        {
            throw new NotImplementedException();
        }

        private void OptionsButtonClicked(object obj)
        {
            //Switching of StartProgramsWindow Height funcionality and dynamically creating/deleting new controls
            if (Application.Current.MainWindow.Height == StartProgramWindowHeightSmall)
            {
                Application.Current.MainWindow.Height = StartProgramWindowHeightBig;
                OptionsButtonContent = "Options <<<";
            }
            else
            {
                Application.Current.MainWindow.Height = StartProgramWindowHeightSmall;
                OptionsButtonContent = "Options >>>";
            }
            
        }

        private void DontStartButtonClicked(object obj)
        {
            throw new NotImplementedException();
        }

        private void StartNowButtonClicked(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
