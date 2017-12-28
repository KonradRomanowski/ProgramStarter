using ProgramStarter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        #region ReadProgramsToStartNamesList
        /// <summary>
        /// This method is reading all program names from xml file and returns them as List
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region ReadProgramsToStartList
        /// <summary>
        /// This method is reading all programs from xml file and returns them as List
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region ReadProgramsToStartCollection
        /// <summary>
        /// This method is reading all programs from xml file and return them as ObservableCollection
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ProgramToStart> ReadProgramsToStartCollection()
        {
            ObservableCollection<ProgramToStart> programsList = new ObservableCollection<ProgramToStart>();
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
        #endregion

        #region SaveProgramsToStartList
        /// <summary>
        /// This method is saving ProgramsToStartList into the XMLFile
        /// </summary>
        /// <param name="newList">List of ProgramToStart which you want to save into xml</param>
        public void SaveProgramsToStartList(ObservableCollection<ProgramToStart> newList)
        {
            XmlDocument doc = new XmlDocument();
            
            if (File.Exists(XMLPath))
            {
                doc.Load(XMLPath);

                XmlNodeList programsToStartNodes = doc.DocumentElement.SelectNodes("/ProgramStarter/ProgramsToStart/Program");

                //clear whole list
                for (int i = programsToStartNodes.Count - 1; i >= 0; i--)
                {
                    programsToStartNodes[i].ParentNode.RemoveChild(programsToStartNodes[i]);
                }

                //create a new one
                foreach (ProgramToStart item in newList)
                {
                    XmlElement childElement = doc.CreateElement("Program");
                    childElement.SetAttribute("order", item.StartingOrder.ToString());
                    childElement.SetAttribute("name", item.ProgramName);
                    childElement.SetAttribute("path", item.Path);
                    XmlNode parentNode = doc.DocumentElement.SelectSingleNode("/ProgramStarter/ProgramsToStart");
                    parentNode.InsertAfter(childElement, parentNode.LastChild);
                }                

                //save the file
                doc.Save(XMLPath);

            }
            else
            {
                throw new Exception("XML file not found at path: " + XMLPath);
            }
        }
        #endregion
    }
}
