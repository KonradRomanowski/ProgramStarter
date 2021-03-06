﻿using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using ProgramStarter.Helpers;
using ProgramStarter.Models;
using ProgramStarter.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IWshRuntimeLibrary;
using System.Windows.Threading;

namespace ProgramStarter.ViewModels
{
    public class StartProgramsViewModel : BaseViewModel, IDropTarget
    {
        #region Statics Definition
        //Version number and year of release
        public static string VersionNumber { get { return "0.0.94"; } }
        public static string YearOfRelease { get { return "2018"; } }

        //Small and big window size
        public static int StartProgramWindowHeightBig = 322;
        public static int StartProgramWindowHeightSmall = 135;

        //Paths
        public static string PathToApp = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        #endregion

        #region Variables Definition

        public ObservableCollection<ProgramToStart> ProgramsToStartList { get; set; }

        XMLHandler configurationFile = new XMLHandler();

        List<Option> optionsList = new List<Option>();

        DispatcherTimer TimeToStart = new DispatcherTimer();

        DispatcherTimer Refresh1sec = new DispatcherTimer();

        StartingProgramsHandler startingProcedure;

        #region ButtonCommands
        public ICommand StartNowButtonCommand { get; private set; }
        public ICommand DontStartButtonCommand { get; private set; }
        public ICommand CancelButtonCommand { get; private set; }
        public ICommand OptionsButtonCommand { get; private set; }
        public ICommand ProgramsToStartButtonCommand { get; private set; }
        public ICommand RemoveProgramFromProgramsToStartList { get; private set; }
        public ICommand AddProgramToProgramsToStartList { get; private set; }
        public ICommand SaveButtonCommand { get; private set; }
        public ICommand TryAgainButtonCommand { get; private set; }
        public ICommand ErrorLogButtonCommand { get; private set; }
        public RelayCommand<Window> ThankYouButtonCommand { get; private set; }
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

        #region Seconds_To_Start_Option
        private string Seconds_To_Start_Option_default = "5";

        private string mSeconds_To_Start_Option;

        public string Seconds_To_Start_Option
        {
            get
            {
                return mSeconds_To_Start_Option;
            }

            set
            {
                if (mSeconds_To_Start_Option == value)
                    return;

                mSeconds_To_Start_Option = value;
                OnPropertyChanged(nameof(Seconds_To_Start_Option));
            }
        }
        #endregion Seconds_To_Start_Option

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

        #region PercentageOfStartedPrograms
        
        private string mPercentageOfStartedPrograms;

        public string PercentageOfStartedPrograms
        {
            get
            {
                return mPercentageOfStartedPrograms;
            }

            set
            {
                if (mPercentageOfStartedPrograms == value)
                    return;

                mPercentageOfStartedPrograms = value;
                OnPropertyChanged(nameof(PercentageOfStartedPrograms));
            }
        }
        #endregion PercentageOfStartedPrograms

        #region Auto_Start_Value
        private bool Auto_Start_Value_default = false;

        private bool mAuto_Start_Value;

        public bool Auto_Start_Value
        {
            get
            {
                return mAuto_Start_Value;
            }

            set
            {
                if (mAuto_Start_Value == value)
                    return;

                mAuto_Start_Value = value;
                OnPropertyChanged(nameof(Auto_Start_Value));
            }
        }
        #endregion Auto_Start_Value

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

        #region SecondsToStartTextBlockVisibility
        private Visibility mSecondsToStartTextBlockVisibility;

        public Visibility SecondsToStartTextBlockVisibility
        {
            get
            {
                return mSecondsToStartTextBlockVisibility;
            }

            set
            {
                if (mSecondsToStartTextBlockVisibility == value)
                    return;

                mSecondsToStartTextBlockVisibility = value;
                OnPropertyChanged(nameof(SecondsToStartTextBlockVisibility));
            }
        }
        #endregion SecondsToStartTextBlockVisibility

        #region StartAndDontStartButtonsVisibility
        private Visibility mStartAndDontStartButtonsVisibility;

        public Visibility StartAndDontStartButtonsVisibility
        {
            get
            {
                return mStartAndDontStartButtonsVisibility;
            }

            set
            {
                if (mStartAndDontStartButtonsVisibility == value)
                    return;

                mStartAndDontStartButtonsVisibility = value;
                OnPropertyChanged(nameof(StartAndDontStartButtonsVisibility));
            }
        }
        #endregion StartAndDontStartButtonsVisibility

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

        #region ProgressBarVisibility
        private Visibility mProgressBarVisibility;

        public Visibility ProgressBarVisibility
        {
            get
            {
                return mProgressBarVisibility;
            }

            set
            {
                if (mProgressBarVisibility == value)
                    return;

                mProgressBarVisibility = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
            }
        }
        #endregion ProgressBarVisibility

        #region FinalizeStartNoErrorsVisibility
        private Visibility mFinalizeStartNoErrorsVisibility;

        public Visibility FinalizeStartNoErrorsVisibility
        {
            get
            {
                return mFinalizeStartNoErrorsVisibility;
            }

            set
            {
                if (mFinalizeStartNoErrorsVisibility == value)
                    return;

                mFinalizeStartNoErrorsVisibility = value;
                OnPropertyChanged(nameof(FinalizeStartNoErrorsVisibility));
            }
        }
        #endregion FinalizeStartNoErrorsVisibility

        #region FinalizeStartWithErrorsVisibility
        private Visibility mFinalizeStartWithErrorsVisibility;

        public Visibility FinalizeStartWithErrorsVisibility
        {
            get
            {
                return mFinalizeStartWithErrorsVisibility;
            }

            set
            {
                if (mFinalizeStartWithErrorsVisibility == value)
                    return;

                mFinalizeStartWithErrorsVisibility = value;
                OnPropertyChanged(nameof(FinalizeStartWithErrorsVisibility));
            }
        }
        #endregion FinalizeStartWithErrorsVisibility       

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

        #region ProgramsToStartListIsEmptyVisibility
        private Visibility mProgramsToStartListIsEmptyVisibility;

        public Visibility ProgramsToStartListIsEmptyVisibility
        {
            get
            {
                return mProgramsToStartListIsEmptyVisibility;
            }

            set
            {
                if (mProgramsToStartListIsEmptyVisibility == value)
                    return;

                mProgramsToStartListIsEmptyVisibility = value;
                OnPropertyChanged(nameof(ProgramsToStartListIsEmptyVisibility));
            }
        }
        #endregion ProgramsToStartListIsEmptyVisibility

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
            SecondsToStartTextBlockVisibility = Visibility.Visible;
            StartAndDontStartButtonsVisibility = Visibility.Visible;
            ProgressBarVisibility = Visibility.Collapsed;
            OptionsGridVisibility = Visibility.Collapsed;
            FinalizeStartNoErrorsVisibility = Visibility.Collapsed;
            FinalizeStartWithErrorsVisibility = Visibility.Collapsed;
            ProgramsToStartListIsEmptyVisibility = Visibility.Collapsed;
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
            CancelButtonCommand = new RelayCommand(CancelButtonClicked);
            OptionsButtonCommand = new RelayCommand(OptionsButtonClicked);
            ProgramsToStartButtonCommand = new RelayCommand(ProgramsToStartButtonClicked);
            RemoveProgramFromProgramsToStartList = new RelayCommand(RemoveProgramContextMenuItemClicked);
            AddProgramToProgramsToStartList = new RelayCommand(AddProgramContextMenuItemClicked);
            SaveButtonCommand = new RelayCommand(SaveButtonClicked);
            TryAgainButtonCommand = new RelayCommand(TryAgainButtonClicked);
            ErrorLogButtonCommand = new RelayCommand(ErrorLogButtonClicked);
            ThankYouButtonCommand = new RelayCommand<Window>(ThankYouButtonClicked);

            //Check if ProgramsToStartList is not empty then start counting seconds to start
            if (ProgramsToStartList.Any())
            {
                //Start counting seconds to start and begin startin procedure of programs when countdown is done
                CountingSecondsToStart();
            }
            else
            {
                //Inform User that list is empty
                ProgramsToStartListIsEmpty();
            }
                       

        }

        #region ProgramsToStartListIsEmpty
        /// <summary>
        /// This method will inform GUI that ProgramsToStart is empty
        /// </summary>
        void ProgramsToStartListIsEmpty()
        {
            ProgramsToStartListIsEmptyVisibility = Visibility.Visible;
            StartAndDontStartButtonsVisibility = Visibility.Visible;

            SecondsToStartTextBlockVisibility = Visibility.Collapsed;
        }

        #endregion

        #region CountingSecondsToStart
        /// <summary>
        /// This method is couting seconds to start programs and fires starting procedure when countdown is finished
        /// </summary>
        private void CountingSecondsToStart()
        {
            TimeToStart.Interval = TimeSpan.FromSeconds(1);
            TimeToStart.Tick += TimeToStart_Tick;
            TimeToStart.Start();
        }

        private void TimeToStart_Tick(object sender, EventArgs e)
        {
            int seconds = Int32.Parse(Seconds_To_Start);

            //if seconds are still more than 0 then count down
            if (seconds > 0)
            {
                seconds--;
                Seconds_To_Start = seconds.ToString();
            }

            //when seconds will be 0 then begin starting procedure
            if (seconds == 0)
            {
                Seconds_To_Start = seconds.ToString();
                TimeToStart.Stop();
                ProgramsStartingProcedure();
            }
            
        }
        #endregion

        #region ProgramsStartingProcedure
        /// <summary>
        /// This method is a starting procedure for all programs in the list
        /// </summary>
        private void ProgramsStartingProcedure()
        {
            //changing values of controls and assigning startup values
            SecondsToStartTextBlockVisibility = Visibility.Collapsed;
            StartAndDontStartButtonsVisibility = Visibility.Collapsed;
            ProgramsToStartListIsEmptyVisibility = Visibility.Collapsed;
            ProgressBarVisibility = Visibility.Visible;
            PercentageOfStartedPrograms = "0";

            //Start of 1 sec timer for refreshing values for UI
            Refresh1sec.Interval = TimeSpan.FromSeconds(1);
            Refresh1sec.Tick += Refresh1sec_Tick;
            Refresh1sec.Start();

            //Starting Procedure
            startingProcedure = new StartingProgramsHandler(ProgramsToStartList.ToList(), Gap_Between_Programs);
            Task.Run(() => startingProcedure.Start());          
            
        }

        /// <summary>
        /// This timer is to refresh values on UI each second during ProgramsStartingProcedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh1sec_Tick(object sender, EventArgs e)
        {
            if (!startingProcedure.IsStartupDone)
            {
                PercentageOfStartedPrograms = startingProcedure.GetPercentOfStartedPrograms().ToString().Split(',').FirstOrDefault();
            }
            else
            {
                PercentageOfStartedPrograms = startingProcedure.GetPercentOfStartedPrograms().ToString().Split(',').FirstOrDefault();
                Refresh1sec.Stop();
                System.Threading.Thread.Sleep(1000);
                FinalizeStartingProcedure();
            }
            
        }
        #endregion

        #region FinalizeStartingProcedure
        /// <summary>
        /// This method will check if any errors occur during starting procedure and inform UI
        /// </summary>
        private void FinalizeStartingProcedure()
        {
            //if no errors during starting procedure
            if (!startingProcedure.HasErrors)
            {
                ProgressBarVisibility = Visibility.Collapsed;
                ProgramsToStartListIsEmptyVisibility = Visibility.Collapsed;
                FinalizeStartNoErrorsVisibility = Visibility.Visible;
            }
            //if errors occured during starting procedure
            else
            {
                ProgressBarVisibility = Visibility.Collapsed;
                ProgramsToStartListIsEmptyVisibility = Visibility.Collapsed;
                FinalizeStartWithErrorsVisibility = Visibility.Visible;
            }
        }
        #endregion

        #region SaveButtonClickedEvent

        private void SaveButtonClicked(object obj)
        {
            //Assign new options to optionsList

            #region Seconds_To_Start_Option
            //Seconds_To_Start_Option
            if (Seconds_To_Start_Option != null)
            {
                if (optionsList.Where(x => x.OptionName == "SecondsToStartPrograms").Count() > 0)
                {
                    //if option already exist then update value
                    foreach (Option item in optionsList.Where(x => x.OptionName == "SecondsToStartPrograms"))
                    {
                        item.ChangeOptionValue(Seconds_To_Start_Option.Trim());
                    }
                }
                else
                {
                    //if option don't exists on the list, add a new one
                    Option item = new Option("SecondsToStartPrograms", Seconds_To_Start_Option.Trim());
                    optionsList.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Option Seconds_To_Start_Option cannot be null - option not saved", "ProgramStarter error");
            }
            #endregion Seconds_To_Start_Option

            #region Gap_Between_Programs
            //Gap_Between_Programs
            if (Gap_Between_Programs != null)
            {
                if (optionsList.Where(x => x.OptionName == "GapBetweenStartingPrograms").Count() > 0)
                {
                    //if option already exist then update value
                    foreach (Option item in optionsList.Where(x => x.OptionName == "GapBetweenStartingPrograms"))
                    {
                        item.ChangeOptionValue(Gap_Between_Programs.Trim());
                    }
                }
                else
                {
                    //if option don't exists on the list, add a new one
                    Option item = new Option("GapBetweenStartingPrograms", Gap_Between_Programs.Trim());
                    optionsList.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Option Gap Between Starting Programs cannot be null - option not saved", "ProgramStarter error");
            }
            #endregion Gap_Between_Programs

            #region Auto_Start_Value
            //Auto_Start_Value

            //First create or delete shortcut file
            //if Auto_Start_Value is true then if shortcut file don't exist - create it
            if (Auto_Start_Value)
            {
                if (!CheckIfAutoStartShortcutExist())
                    CreateAutoStartShortcut();                
            }
            else //else (Auto_Start_Value is false) then if shortcut file exist - delete it
            {
                if (CheckIfAutoStartShortcutExist())
                {
                    try
                    {
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\ProgramStarter.lnk");
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Error during SaveButtonClicked - delete AutoStart shortcut: " + err);
                    }
                }                    
            }

            //Then change option value on optionsList
            if (Auto_Start_Value != null)
            {
                string _value = (Auto_Start_Value) ? "true" : "false";

                if (optionsList.Where(x => x.OptionName == "AutoStart").Count() > 0)
                {                    
                    //if option already exist then update value
                    foreach (Option item in optionsList.Where(x => x.OptionName == "AutoStart"))
                    {                        
                        item.ChangeOptionValue(_value);
                    }
                }
                else
                {                    
                    //if option don't exists on the list, add a new one
                    Option item = new Option("AutoStart", Gap_Between_Programs);
                    optionsList.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Option AutoStart cannot be null - option not saved", "ProgramStarter error");
            }
            #endregion Auto_Start_Value

            //save the configuration file
            SavingConfigurationFile();
        }

        #endregion

        #region CreateAutoStartShortcut
        /// <summary>
        /// This method is creating shortcut to app in AutoStart folder
        /// </summary>
        private void CreateAutoStartShortcut()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\ProgramStarter.lnk";

            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.Description = "ProgramStarter Shortcut";
                shortcut.TargetPath = PathToApp + @"\ProgramStarter.exe";
                shortcut.WorkingDirectory = PathToApp;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying method CreateAutoStartShortcut(): " + ex, "ProgramStarter error");               
            }
        }

        #endregion

        #region CheckIfAutoStartShortcutExist
        /// <summary>
        /// This method is checking if AutoStart shortcut exists or not
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAutoStartShortcutExist()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\ProgramStarter.lnk";

            return (System.IO.File.Exists(shortcutPath)) ? true : false;
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
            TimeToStart.Stop();
            SecondsToStartTextBlockVisibility = Visibility.Hidden;
        }

        private void StartNowButtonClicked(object obj)
        {
            TimeToStart.Stop();
            if (ProgramsToStartList.Any())
            {
                ProgramsStartingProcedure();
            }
            else
            {
                ProgramsToStartListIsEmpty();
            }
        }
        #endregion

        #region Cancel button clicked event

        private void CancelButtonClicked(object obj)
        {
            startingProcedure.StopStartingProcedure();
            Refresh1sec.Stop();

            //Finalize starting procedue with errors
            ProgressBarVisibility = Visibility.Collapsed;
            FinalizeStartWithErrorsVisibility = Visibility.Visible;
        }
        #endregion

        #region TryAgain button clicked event

        private void TryAgainButtonClicked(object obj)
        {
            //Setting proper visibility
            FinalizeStartNoErrorsVisibility = Visibility.Collapsed;
            FinalizeStartWithErrorsVisibility = Visibility.Collapsed;

            //Start programs again
            if (ProgramsToStartList.Any())
            {
                ProgramsStartingProcedure();
            }
            else
            {
                ProgramsToStartListIsEmpty();
            }
        }
        #endregion

        #region ErrorLog Button Clicked event

        private void ErrorLogButtonClicked(object obj)
        {
            ShowErrorLogViewModel vm = new ShowErrorLogViewModel(startingProcedure.ErrorsList);
            Window win = new ShowErrorLogWindow();
            win.DataContext = vm;            
            win.Show();
        }
        #endregion

        #region ThankYou Button Clicked event

        public void ThankYouButtonClicked(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
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

            //Saving Options
            try
            {
                configurationFile.SaveOptionsListToXML(optionsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying method SavingConfigurationFile - Saving Options: " + ex, "ProgramStarter error");
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

            if (optionsList.Any())
            {
                //Seconds_To_Start & Seconds_To_Start_Option
                try
                {
                    Seconds_To_Start = (optionsList.Where(x => x.OptionName == "SecondsToStartPrograms").Count() > 0) ?
                        optionsList.Where(x => x.OptionName == "SecondsToStartPrograms").Select(x => x.OptionValue).First()
                        : Seconds_To_Start_default;

                    Seconds_To_Start_Option = (optionsList.Where(x => x.OptionName == "SecondsToStartPrograms").Count() > 0) ?
                        optionsList.Where(x => x.OptionName == "SecondsToStartPrograms").Select(x => x.OptionValue).First()
                        : Seconds_To_Start_Option_default;
                }
                catch (Exception ex)
                {
                    //Assign default value
                    Seconds_To_Start = Seconds_To_Start_default;
                    MessageBox.Show("An error occured while trying method ReadingOptionsToVariables(Assigning readed options to variables Seconds_To_Start & Seconds_To_Start_Option, used default value): " + ex, "ProgramStarter error");
                }

                //Gap_Between_Programs
                try
                {
                    Gap_Between_Programs = (optionsList.Where(x => x.OptionName == "GapBetweenStartingPrograms").Count() > 0) ?
                        optionsList.Where(x => x.OptionName == "GapBetweenStartingPrograms").Select(x => x.OptionValue).First()
                        : Gap_Between_Programs_default;
                }
                catch (Exception ex)
                {
                    //Assign default value
                    Gap_Between_Programs = Gap_Between_Programs_default;
                    MessageBox.Show("An error occured while trying method ReadingOptionsToVariables(Assigning readed options to variable Gap_Between_Programs, used default value): " + ex, "ProgramStarter error");
                }

                //Auto_Start_Value
                try
                {
                    //check if shortcut exists in startup folder 
                    Auto_Start_Value = (CheckIfAutoStartShortcutExist()) ? true : false;
                }
                catch (Exception ex)
                {
                    Auto_Start_Value = Auto_Start_Value_default;
                    MessageBox.Show("An error occured while trying method ReadingOptionsToVariables(Assigning readed options to variable Auto_Start_Value, used default value): " + ex, "ProgramStarter error");
                }
            }            
        }
        #endregion        
    }
}
