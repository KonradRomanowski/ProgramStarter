using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramStarter.Models
{
    public class Option
    {
        public string OptionName { get; private set; }
        public string OptionValue { get; private set; }

        public Option(string optionName, string optionValue)
        {
            OptionName = optionName;
            OptionValue = optionValue;
        }

        public void ChangeOptionValue(string value)
        {
            OptionValue = value;
        }
    }    
}
