using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramStarter.Models
{
    public class ProgramToStart
    {
        public string ProgramName { get; private set; }
        public string Path { get; private set; }
        public int StartingOrder { get; set; }

        public ProgramToStart(string _programName, string _programPath)
        {
            ProgramName = _programName;
            Path = _programPath;
        }

        public void ChangeProgramName(string _programName)
        {
            ProgramName = _programName;
        }

        public void ChangeProgramPath(string _programPath)
        {
            Path = _programPath;
        }

    }
}
