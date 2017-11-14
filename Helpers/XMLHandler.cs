using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProgramStarter.Helpers
{
    public class XMLHandler
    {
        public string XMLPath { get; set; }

        public XMLHandler(string _xmlPath)
        {
            XMLPath = _xmlPath;
        }

        public List<string> ReadProgramsToStartNamesList()
        {
            List<string> programsList = new List<string>();
            XmlDocument doc = new XmlDocument();

            if (File.Exists(XMLPath))
            {
                doc.Load(XMLPath);

                XmlNodeList programsToStartNodes = doc.DocumentElement.SelectNodes("/ProgramStarter/ProgramsToStart/Program");

                foreach (XmlNode programNode in programsToStartNodes)
                {
                    string _program = programNode.Attributes["name"].Value;
                    programsList.Add(_program);
                }                
            }

            return programsList;

        }

    }
}
