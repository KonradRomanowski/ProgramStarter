﻿using GongSolutions.Wpf.DragDrop;
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
        public static int StartProgramWindowHeightBig = 305;
        public static int StartProgramWindowHeightSmall = 135;
        #endregion

        #region Variables Definition

        public ObservableCollection<ProgramToStart> ProgramsToStartList { get; set; }   
    
        #region ButtonCommands
        public ICommand StartNowButtonCommand { get; private set; }
        public ICommand DontStartButtonCommand { get; private set; }
        public ICommand OptionsButtonCommand { get; private set; }
        public ICommand ProgramsToStartButtonCommand { get; private set; }
        public ICommand RemoveProgramFromProgramsToStartList { get; private set; }
        public ICommand AddProgramToProgramsToStartList { get; private set; }
        #endregion ButtonCommands

        #region Seconds_To_Start
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

            //just for tests - can be deleted  - using of XMLHandler to read Programs to start from xml          
            XMLHandler test = new XMLHandler(@"D:\Programy Programowanie\Moje\ProgramStarter\csharp\ProgramStarter\ProgramStarter\Data\configuration.xml");
            ProgramsToStartList = test.ReadProgramsToStartCollection();
            //ProgramsToStartList.CollectionChanged += ProgramsToStartList_CollectionChanged;            

            //foreach (var item in ttt)
            //{
            //    MessageBoxResult result = MessageBox.Show(item.StartingOrder.ToString() + ' ' + item.ProgramName + ' ' + item.Path);
            //}
            //------

            //Binding for buttons
            StartNowButtonCommand = new RelayCommand(StartNowButtonClicked);
            DontStartButtonCommand = new RelayCommand(DontStartButtonClicked);
            OptionsButtonCommand = new RelayCommand(OptionsButtonClicked);
            ProgramsToStartButtonCommand = new RelayCommand(ProgramsToStartButtonClicked);
            RemoveProgramFromProgramsToStartList = new RelayCommand(RemoveProgramContextMenuItemClicked);
            AddProgramToProgramsToStartList = new RelayCommand(AddProgramContextMenuItemClicked);

        }

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
                ProgramsToStartList.Insert(SelectedProgramOnProgramsToStartListView.StartingOrder - 1, program);
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
    }
}
