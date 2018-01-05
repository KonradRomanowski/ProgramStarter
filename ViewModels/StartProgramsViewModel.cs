using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using ProgramStarter.Helpers;
using ProgramStarter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProgramStarter.ViewModels
{
    public class StartProgramsViewModel : BaseViewModel, IDropTarget
    {
        #region Statics Definition
        //Version number and year of release
        public static string VersionNumber { get { return "0.0.90"; } }
        public static string YearOfRelease { get { return "2017"; } }
        //Small and big window size
        public static int StartProgramWindowHeightBig = 322;
        public static int StartProgramWindowHeightSmall = 135;
        #endregion

        #region Variables Definition

        public ObservableCollection<ProgramToStart> ProgramsToStartList { get; set; }

        XMLHandler configurationFile = new XMLHandler();

        List<Option> optionsList = new List<Option>();

        #region ButtonCommands
        public ICommand StartNowButtonCommand { get; private set; }
        public ICommand DontStartButtonCommand { get; private set; }
        public ICommand OptionsButtonCommand { get; private set; }
        public ICommand ProgramsToStartButtonCommand { get; private set; }
        public ICommand RemoveProgramFromProgramsToStartList { get; private set; }
        public ICommand AddProgramToProgramsToStartList { get; private set; }
        public ICommand SaveButtonCommand { get; private set; }
        #endregion ButtonCommands

        #region Seconds_To_Start
        private string Seconds_To_Start_default = "5";

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
        #endregion Seconds_To_Start

        #region Gap_Between_Programs
        private string Gap_Between_Programs_default = "1";

        private string mGap_Between_Programs;

        public string Gap_Between_Programs
        {
            get
            {
                return mGap_Between_Programs;
            }

            set
            {
                if (mGap_Between_Programs == value)
                    return;

                mGap_Between_Programs = value;
                OnPropertyChanged(nameof(Gap_Between_Programs));
            }
        }
        #endregion Gap_Between_Programs

        #region OptionsButtonContent
        private string mOptionsButtonContent;

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
        #endregion OptionsButtonContent

        #region OptionsGridVisibility
        private Visibility mOptionsGridVisibility;

        public Visibility OptionsGridVisibility
        {
            get
            {
                return mOptionsGridVisibility;
            }

            set
            {
                if (mOptionsGridVisibility == value)
                    return;

                mOptionsGridVisibility = value;
                OnPropertyChanged(nameof(OptionsGridVisibility));
            }
        }
        #endregion OptionsGridVisibility

        #region ProgramsToStartButtonContent
        private string mProgramsToStartButtonContent;

        public string ProgramsToStartButtonContent
        {
            get
            {
                return mProgramsToStartButtonContent;
            }

            set
            {
                if (mProgramsToStartButtonContent == value)
                    return;

                mProgramsToStartButtonContent = value;
                OnPropertyChanged(nameof(ProgramsToStartButtonContent));
            }
        }
        #endregion ProgramsToStartButtonContent

        #region ProgramsToStartGridVisibility
        private Visibility mProgramsToStartGridVisibility;

        public Visibility ProgramsToStartGridVisibility
        {
            get
            {
                return mProgramsToStartGridVisibility;
            }

            set
            {
                if (mProgramsToStartGridVisibility == value)
                    return;

                mProgramsToStartGridVisibility = value;
                OnPropertyChanged(nameof(ProgramsToStartGridVisibility));
            }
        }
        #endregion ProgramsToStartGridVisibility

        #region SelectedProgramOnProgramsToStartListView
        //SelectedProgramOnProgramsToStartListView property is checking which item is selected on ProgramsToStartListView
        ProgramToStart _selectedProgramOnProgramsToStartListView;

        public ProgramToStart SelectedProgramOnProgramsToStartListView
        {
            get { return _selectedProgramOnProgramsToStartListView; }
            set
            {
                if (value != _selectedProgramOnProgramsToStartListView)
                {
                    _selectedProgramOnProgramsToStartListView = value;
                    this.OnPropertyChanged("SelectedProgramOnProgramsToStartListView");
                }
            }
        }
        #endregion SelectedProgramOnProgramsToStartListView

        #endregion Variables Definition


        public StartProgramsViewModel()
        {            
            //Assigning startup values for controls
            OptionsGridVisibility = Visibility.Collapsed;
            OptionsButtonContent = "Options >>>";
            ProgramsToStartGridVisibility = Visibility.Collapsed;
            ProgramsToStartButtonContent = "Programs to Start >>>";

            //Obtain configuration.xml path
            ObtainingConfigurationXMLPath();

            //Read all saved Programs from configuration file and assign them to ProgramsToStartList               
            ReadingProgramsToStartCollection();

            //Read all options from configuration file and assign them to proper variables
            ReadingOptionsToVariables();
            

            //Binding for buttons
            StartNowButtonCommand = new RelayCommand(StartNowButtonClicked);
            DontStartButtonCommand = new RelayCommand(DontStartButtonClicked);
            OptionsButtonCommand = new RelayCommand(OptionsButtonClicked);
            ProgramsToStartButtonCommand = new RelayCommand(ProgramsToStartButtonClicked);
            RemoveProgramFromProgramsToStartList = new RelayCommand(RemoveProgramContextMenuItemClicked);
            AddProgramToProgramsToStartList = new RelayCommand(AddProgramContextMenuItemClicked);
            SaveButtonCommand = new RelayCommand(SaveButtonClicked);

        }

        #region SaveButtonClickedEvent

        private void SaveButtonClicked(object obj)
        {
            SavingConfigurationFile();
        }

        #endregion

        #region ProgramsToStartList_CollectionChanged
        private void ProgramsToStartList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (ProgramToStart item in ProgramsToStartList)
                {
                    OnPropertyChanged(nameof(ProgramToStart));
                }
                
            }

            if (e.NewItems != null)
            {
                foreach (ProgramToStart item in ProgramsToStartList)
                {
                    OnPropertyChanged(nameof(ProgramToStart));
                }

            }
        }
        #endregion

        #region Add/Remove ProgramsToStart ContextMenu Buttons

        private void AddProgramContextMenuItemClicked(object obj)
        {
            //TODO: This is breaking the MVVM concept, need to figure out how to this in MVVM style            
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                ProgramToStart program = new ProgramToStart(openFileDialog.SafeFileName, openFileDialog.FileName);

                //check if user clicked on program or on some empty field in ListView
                //if user clicked on program then insert new program before the selected item
                if (SelectedProgramOnProgramsToStartListView != null)
                {
                    ProgramsToStartList.Insert(SelectedProgramOnProgramsToStartListView.StartingOrder - 1, program);
                }
                else //if user clicked on some empty field then add new program at the end of the list
                {
                    ProgramsToStartList.Add(program);
                }
                
                UpdateStartingOrdersInProgramsToStartListView();
                RefreshProgramsToStartListView();
            }
            
        }

        private void RemoveProgramContextMenuItemClicked(object obj)
        {
            ProgramsToStartList.Remove(SelectedProgramOnProgramsToStartListView);
            UpdateStartingOrdersInProgramsToStartListView();
            RefreshProgramsToStartListView();
        }

        #endregion

        #region ProgramsToStart and Options buttons clicked events
        private void ProgramsToStartButtonClicked(object obj)
        {
            //Switching of StartProgramsWindow Height funcionality and dynamically changing visibility of Programs To Start Grid
            if (ProgramsToStartGridVisibility != Visibility.Visible)
            {
                OptionsGridVisibility = Visibility.Collapsed;
                OptionsButtonContent = "Options >>>";
                Application.Current.MainWindow.Height = StartProgramWindowHeightBig;
                ProgramsToStartGridVisibility = Visibility.Visible;
                ProgramsToStartButtonContent = "Programs to Start <<<";                
            }
            else
            {
                Application.Current.MainWindow.Height = StartProgramWindowHeightSmall;
                ProgramsToStartGridVisibility = Visibility.Collapsed;
                ProgramsToStartButtonContent = "Programs to Start >>>";
            }
        }

        private void OptionsButtonClicked(object obj)
        {
            //Switching of StartProgramsWindow Height funcionality and dynamically changing visibility of Options Grid
            if (OptionsGridVisibility != Visibility.Visible)
            {
                ProgramsToStartGridVisibility = Visibility.Collapsed;
                ProgramsToStartButtonContent = "Programs to Start >>>";
                Application.Current.MainWindow.Height = StartProgramWindowHeightBig;
                OptionsGridVisibility = Visibility.Visible;
                OptionsButtonContent = "Options <<<";                
            }
            else
            {
                Application.Current.MainWindow.Height = StartProgramWindowHeightSmall;
                OptionsGridVisibility = Visibility.Collapsed;
                OptionsButtonContent = "Options >>>";
            }
            
        }
        #endregion

        #region StartNow and DontStart buttons clicked events

        private void DontStartButtonClicked(object obj)
        {
            throw new NotImplementedException();
        }

        private void StartNowButtonClicked(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Drag and Drop methods for ProgramsToStartListView

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;            
        }

        public void Drop(IDropInfo dropInfo)
        {
            ProgramToStart sourceItem = dropInfo.Data as ProgramToStart;   //dragged item
            ProgramToStart targetItem = dropInfo.TargetItem as ProgramToStart;   //item on with user drops the sourceitem
            RelativeInsertPosition positionOfItem = dropInfo.InsertPosition;   //position (before or after targetItem)
            int insertIndex = dropInfo.InsertIndex;   //positon in ProgramsToStartList where item was dropped            

            //Checking if item was dropped before or after targetItem and moving into correct position in ProgramsToStartList
            if (positionOfItem == (RelativeInsertPosition.BeforeTargetItem | RelativeInsertPosition.TargetItemCenter))
            {
                ProgramsToStartList.Move(sourceItem.StartingOrder - 1, insertIndex);
            }
            else
            {
                //Checking if new position is the last in the list - if it is last, put item in the last position
                //To prevent ArgumentOutOfRange Exception for ProgramsToStartList.Move()   
                if (insertIndex == ProgramsToStartList.Count)
                {
                    ProgramsToStartList.Move(sourceItem.StartingOrder - 1, insertIndex - 1);
                }
                else
                {
                    ProgramsToStartList.Move(sourceItem.StartingOrder - 1, insertIndex);
                }

            }

            //After source Item was moved to the new position we need to update StartingOrder property of all ProgramsToStart
            UpdateStartingOrdersInProgramsToStartListView();

            //Then we need to refresh the ListView
            RefreshProgramsToStartListView();
            
        }
        #endregion

        #region Update of Programs StartingOrders in ProgramsToStartListView

        public void UpdateStartingOrdersInProgramsToStartListView()
        {
            for (int i = 0; i < ProgramsToStartList.Count; i++)
            {
                ProgramsToStartList[i].StartingOrder = i + 1;
            }
        }

        #endregion

        #region Refresh of ProgramsToStartListView

        public void RefreshProgramsToStartListView()
        {
            //
            //TODO: This is a temporary solution, need to figure out how to refresh ListView without clearing whole list
            ObservableCollection<ProgramToStart> _temp = new ObservableCollection<ProgramToStart>();
            foreach (ProgramToStart item in ProgramsToStartList)
            {
                ProgramToStart insert = new ProgramToStart(item.ProgramName, item.Path);
                insert.StartingOrder = item.StartingOrder;
                _temp.Add(insert);
            }

            ProgramsToStartList.Clear();
            foreach (ProgramToStart item in _temp)
            {
                ProgramToStart insert = new ProgramToStart(item.ProgramName, item.Path);
                insert.StartingOrder = item.StartingOrder;
                ProgramsToStartList.Add(insert);
            }
        }
        #endregion

        #region SavingConfigurationFile
        /// <summary>
        /// Method for saving configuration.xml file
        /// </summary>
        public void SavingConfigurationFile()
        {
            //Saving Programs 
            try
            {
                configurationFile.SaveProgramsToStartList(ProgramsToStartList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying method SavingConfigurationFile - Saving Programs: " + ex, "ProgramStarter error");               
            }
            
        }
        #endregion

        #region ReadingProgramsToStartCollection
        /// <summary>
        /// This method is for reading ProgramsToStart from xml file
        /// </summary>
        public void ReadingProgramsToStartCollection()
        {
            try
            {
                ProgramsToStartList = configurationFile.ReadProgramsToStartCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying method ReadingProgramsToStartCollection: " + ex, "ProgramStarter error");
            }            
        }

        #endregion

        #region ObtainingConfigurationXMLPath
        /// <summary>
        /// This method is calling ObtainXMLPath from XMLHandler and catching exceptions
        /// </summary>
        public void ObtainingConfigurationXMLPath()
        {
            try
            {
                configurationFile.ObtainXMLPath();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying method ObtainingConfigurationXMLPath: " + ex, "ProgramStarter error");
            }            
        }
        #endregion

        #region ReadingOptionsToVariables
        /// <summary>
        /// This method is assigning options readed from XMLFile to proper variables
        /// </summary>
        public void ReadingOptionsToVariables()
        {
            //First read all options from configuration file
            try
            {
                optionsList = configurationFile.ReadOptionsFromConfigurationXML();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying method ReadingOptionsToVariables(Reading options from configuration file): " + ex, "ProgramStarter error");
            }

            //Then try to assign readed options to proper variables (if not readed assign default value)

            //Seconds_To_Start
            try
            {
                Seconds_To_Start = optionsList.Where(x => x.OptionName == "SecondsToStartPrograms").Select(x => x.OptionValue).First();                
            }
            catch (Exception ex)
            {
                //Assign default value
                Seconds_To_Start = Seconds_To_Start_default;
                MessageBox.Show("An error occured while trying method ReadingOptionsToVariables(Assigning readed options to variable Seconds_To_Start, used default value): " + ex, "ProgramStarter error");
            }

            //Gap_Between_Programs
            try
            {
                Gap_Between_Programs = optionsList.Where(x => x.OptionName == "GapBetweenStartingPrograms").Select(x => x.OptionValue).First();
            }
            catch (Exception ex)
            {
                //Assign default value
                Gap_Between_Programs = Gap_Between_Programs_default;
                MessageBox.Show("An error occured while trying method ReadingOptionsToVariables(Assigning readed options to variable Gap_Between_Programs, used default value 1): " + ex, "ProgramStarter error");
            }

        }
        #endregion        
    }
}
