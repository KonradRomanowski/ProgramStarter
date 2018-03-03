using ProgramStarter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramStarter.ViewModels
{
    public class ShowErrorLogViewModel : BaseViewModel
    {
        #region Variables Definition

        /// <summary>
        /// List with all Error Logs
        /// </summary>
        public ObservableCollection<ErrorLog> ErrorLogsList { get; set; }

        #endregion Variables Definition

        public ShowErrorLogViewModel(List<ErrorLog> _errorLogsList)
        {
            ErrorLogsList = new ObservableCollection<ErrorLog>();

            //assing all ErrorLogs from _errorLogsList to ObservableCollection ErrorLogsList
            foreach (ErrorLog log in _errorLogsList)
            {
                ErrorLogsList.Add(log);
            }
        }
    }
}
