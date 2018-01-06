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
        public string XMLPath { get; private set; }

        public XMLHandler()
        {
            
        }

        #region ObtainXMLPath
        /// <summary>
        /// This method is obtaining XMLPath for XMLHandler - must be used before all other methods
        /// </summary>
        public void ObtainXMLPath()
        {
            //if 'try catch' fails then XMLPath will be null
            XMLPath = null;

            try
            {
                XMLPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                XMLPath = Path.Combine(XMLPath, "Data\\configuration.xml");
                XMLPath = new Uri(XMLPath).LocalPath;  //this will cut 'file:///' at the beginning of path from .CodeBase method
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected exception with XMLPath (XMLPath set to null): " + ex.ToString());
            }           

        }
        #endregion

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

            //if XMLPath is null throw exception
            if (XMLPath == null)
            {
                throw new Exception("XMLPath is null");
            }

            if (File.Exists(XMLPath))
            {
                doc.Load(XMLPath);

                XmlNodeList programsToStartNodes = doc.DocumentElement.SelectNodes("/ProgramStarter/ProgramsToStart/Program");

                foreach (XmlNode programNode in programsToStartNodes)
                {
                    if (programNode.Attributes["name"] != null && programNode.Attributes["path"] != null && programNode.Attributes["order"] != null)
                    {
                        ProgramToStart _program = new ProgramToStart(programNode.Attributes["name"].Value, programNode.Attributes["path"].Value);
                        if (int.TryParse(programNode.Attributes["order"].Value, out temp))
                            _program.StartingOrder = int.Parse(programNode.Attributes["order"].Value);
                        else
                            throw new Exception("Starting Order could not be parsed into int for: " + _program.ProgramName);
                        programsList.Add(_program);
                    }
                    else
                    {
                        throw new Exception("One of Program To Start in configuration.xml doesn't have name, path or order (Null Exception)");
                    }                    
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

            //if XMLPath is null throw exception
            if (XMLPath == null)
            {
                throw new Exception("XMLPath is null");
            }
            
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
        
        #region ReadOptionsFromConfigurationXML
        /// <summary>
        /// This method is reading all options from configuration.xml file and returns them as a List of type Option
        /// </summary>
        /// <returns></returns>
        public List<Option> ReadOptionsFromConfigurationXML()
        {
            List<Option> readedOptions = new List<Option>();
            XmlDocument doc = new XmlDocument();

            //if XMLPath is null throw exception
            if (XMLPath == null)
            {
                throw new Exception("XMLPath is null");
            }

            if (File.Exists(XMLPath))
            {
                doc.Load(XMLPath);

                XmlNodeList optionsNodes = doc.DocumentElement.SelectNodes("/ProgramStarter/ProgramSettings/Setting");

                foreach (XmlNode option in optionsNodes)
                {
                    if (option.Attributes["name"] != null && option.Attributes["value"] != null)
                    {
                        Option _option = new Option(option.Attributes["name"].Value, option.Attributes["value"].Value);
                        readedOptions.Add(_option);
                    }
                    else
                    {
                        throw new Exception("One of Program Settings in configuration.xml doesn't have name or value (Null Exception)");
                    }                    
                }
            }
            else
            {
                throw new Exception("XML file not found at path: " + XMLPath);
            }

            return readedOptions;
        }
        #endregion

        #region SaveOptionsListToXML
        /// <summary>
        /// This method will save new options list into XML file
        /// </summary>
        /// <param name="newOptionsList">List of type Option with new user settings to save</param>
        public void SaveOptionsListToXML(List<Option> newOptionsList)
        {
            XmlDocument doc = new XmlDocument();

            //if XMLPath is null throw exception
            if (XMLPath == null)
            {
                throw new Exception("XMLPath is null");
            }

            if (File.Exists(XMLPath))
            {
                doc.Load(XMLPath);

                XmlNodeList optionsNodes = doc.DocumentElement.SelectNodes("/ProgramStarter/ProgramsToStart/ProgramSettings");

                //clear whole list
                for (int i = optionsNodes.Count - 1; i >= 0; i--)
                {
                    optionsNodes[i].ParentNode.RemoveChild(optionsNodes[i]);
                }

                //create a new one
                foreach (Option item in newOptionsList)
                {
                    XmlElement childElement = doc.CreateElement("Setting");
                    childElement.SetAttribute("name", item.OptionName);
                    childElement.SetAttribute("value", item.OptionValue);
                    XmlNode parentNode = doc.DocumentElement.SelectSingleNode("/ProgramStarter/ProgramSettings");
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
