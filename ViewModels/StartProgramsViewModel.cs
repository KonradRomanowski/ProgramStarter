using ProgramStarter.Helpers;
using ProgramStarter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProgramStarter.ViewModels
{
    public class StartProgramsViewModel : BaseViewModel
    {
        private string mSeconds_To_Start;  

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


        public StartProgramsViewModel()
        {
            //just for tests - can be deleted
            XMLHandler test = new XMLHandler(@"D:\Programy Programowanie\Moje\ProgramStarter\csharp\ProgramStarter\ProgramStarter\Data\configuration.xml");
            List<ProgramToStart> ttt = test.ReadProgramsToStartList();

            foreach (var item in ttt)
            {
                MessageBoxResult result = MessageBox.Show(item.StartingOrder.ToString() + ' ' + item.ProgramName + ' ' + item.Path);
            }

        }       

    }
}
