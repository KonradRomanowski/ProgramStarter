using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramStarter.Models
{
    /// <summary>
    /// Error log for logging errors during starting of programs procedure
    /// </summary>
    public class ErrorLog
    {
        public DateTime DateAndTime { get; private set; }

        public string ProgramName { get; private set; }

        public string ProgramPath { get; private set; }

        public string ErrorDescription { get; private set; }

        /// <summary>
        /// Creates a new error log for starting of programs procedure
        /// </summary>
        /// <param name="_dateandtime">Date and time when error occurs</param>
        /// <param name="_programname">Program name</param>
        /// <param name="_programpath">Path</param>
        /// <param name="_errordescription">Error description</param>
        public ErrorLog(DateTime _dateandtime, string _programname, string _programpath, string _errordescription)
        {
            DateAndTime = _dateandtime;
            ProgramName = _programname;
            ProgramPath = _programpath;
            ErrorDescription = _errordescription;
        }
    }
}
