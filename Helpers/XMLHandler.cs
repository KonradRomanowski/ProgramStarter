using ProgramStarter.Models;
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
            else
            {
                throw new Exception("XML file not found at path: " + XMLPath);
            }

            return programsList;

        }

        public List<ProgramToStart> ReadProgramsToStartList()
        {
            List<ProgramToStart> programsList = new List<ProgramToStart>();
            XmlDocument doc = new XmlDocument();
            int temp = 0;

            if (File.Exists(XMLPath))
            {
                doc.Load(XMLPath);

                XmlNodeList programsToStartNodes = doc.DocumentElement.SelectNodes("/ProgramStarter/ProgramsToStart/Program");

                foreach (XmlNode programNode in programsToStartNodes)
                {
                    ProgramToStart _program = new ProgramToStart(programNode.Attributes["name"].Value, programNode.Attributes["path"].Value);
                    if (int.TryParse(programNode.Attributes["order"].Value, out temp))
                        _program.StartingOrder = int.Parse(programNode.Attributes["order"].Value);
                    else
                        throw new Exception("Starting Order could not be parsed into int for: " + _program.ProgramName);
                    programsList.Add(_program);
                }
            }
            else
            {
                throw new Exception("XML file not found at path: " + XMLPath);
            }

            return programsList;
        }

    }
}
