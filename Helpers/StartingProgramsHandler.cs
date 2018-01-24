﻿using ProgramStarter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ProgramStarter.Helpers
{
    public class StartingProgramsHandler
    {
        #region Fields&Properties
        DispatcherTimer GapCountTimer = new DispatcherTimer();
        
        private List<ProgramToStart> ProgramsToStartList { get; set; }   
        private float PercentOfStartedPrograms { get; set; }  
        private int GapBetweenPrograms { get; set; }

        #region CurrentProgramStartingOrder
        /// <summary>
        /// This property is the Starting Order value of current program waiting to be started
        /// </summary>
        public int CurrentProgramStartingOrder { get; private set; }

        #endregion CurrentProgramStartingOrder

        #region IsStartupDone
        /// <summary>
        /// This property is set to true when startup of all programs is done
        /// </summary>
        public bool IsStartupDone { get; private set; }
        #endregion IsStartupDone

        #endregion Fields&Properties

        public StartingProgramsHandler(List<ProgramToStart> _programsToStartList, string _gapBetweenPrograms)
        {
            //Assigning values from ctor arguments
            ProgramsToStartList = _programsToStartList.OrderBy(x => x.StartingOrder).ToList();
            GapBetweenPrograms = Int32.Parse(_gapBetweenPrograms);

            //Setting start values for properties
            CurrentProgramStartingOrder = 0;
            PercentOfStartedPrograms = 0;

            //Setting timer for starting programs
            GapCountTimer.Interval = TimeSpan.FromSeconds(GapBetweenPrograms);
            GapCountTimer.Tick += GapCountTimer_Tick;
        }

        #region Start
        /// <summary>
        /// Begining starting procedure for all programs in the list
        /// </summary>
        public void Start()
        {
            //Start first program in the list 
            StartNextProgram();

            //Start GapCountTimer and every timer tick next program will be started
            GapCountTimer.Start();
        }
        #endregion

        #region StartNextProgram
        /// <summary>
        /// This method is starting next program in the list, calculating PercentOfStartedPrograms and incrementing CurrentProgramStartingOrder
        /// </summary>
        private void StartNextProgram()
        {
            //Start program
            //TODO

            //Calculate percentage of started programs
            PercentOfStartedPrograms = ((float) CurrentProgramStartingOrder / ProgramsToStartList.Count()) * 100;

            //Increment CurrentProgramStartingOrder
            CurrentProgramStartingOrder++;            
        }
        #endregion

        #region GapCountTimer_Tick
        /// <summary>
        /// Timer_Tick event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GapCountTimer_Tick(object sender, EventArgs e)
        {
            //If there are still programs to start on the list then start the next one on the list
            if (CurrentProgramStartingOrder < ProgramsToStartList.Count())
                StartNextProgram();
            //else stop the starting procedure
            else
            {
                IsStartupDone = true;
                PercentOfStartedPrograms = ((float)CurrentProgramStartingOrder / ProgramsToStartList.Count()) * 100;
                GapCountTimer.Stop();
            }
                
        }
        #endregion

        #region GetPercentOfStartedPrograms
        /// <summary>
        /// This method is returning percentage of already started programs
        /// </summary>
        /// <returns></returns>
        public float GetPercentOfStartedPrograms()
        {
            return PercentOfStartedPrograms;
        }

        #endregion
    }
}
