using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Curve;
using log4net;
using Model;
using UnityEngine;
using Views;
using Views.Exercise;

namespace Controller.Exercise
{
    /// <summary>
    /// Controller for in-game views of exercise related data structures
    /// </summary>
    public class ExerciseCurveViewController : AbstractExerciseViewController
    {
        #region Public members

        /// <summary>
        /// Chosen selections in selection exercise, all -1 per default to represent non-choice
        /// </summary>
        public readonly List<int> SelectionIndices = new List<int>();

        #endregion Public members
        
        #region Private members

        /// <summary>
        /// Data model of the controller, representing a selection exercise
        /// </summary>
        private static SelectionExercise CurrentExercise => 
            GlobalDataModel.SelectionExercises.Any() 
                ? GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex] : null;
        
        /// <summary>
        /// Static log4net logger instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractExerciseViewController));
        
        #endregion Private members
        
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="root">Root transform</param>
        /// <param name="selObjs">Selectable objects</param>
        /// <param name="pillarPrefab">Pillar prefab</param>
        /// <param name="type">Controller type</param>
        public ExerciseCurveViewController(Transform root, SelectionExerciseGameObjects selObjs, GameObject pillarPrefab,
            AbstractCurveViewController.CurveControllerType type) : base(root)
        {
            // Return if no exercises were imported
            if (CurrentExercise == null) return;

            // Initialize all answers as 'none given'
            for (var i = 0; i < CurrentExercise.CorrectAnswers.Count; i++)
            {
                SelectionIndices.Add(-1);
            }
            
            var selView = new SelectionExerciseCompoundView(selObjs, pillarPrefab, root, 
                CurrentExercise.Datasets[0], type)
            {
                CurrentTitle = CurrentExercise.Title,
                CurrentDescription = CurrentExercise.Description
            };

            Views = new List<AbstractExerciseView>()
            {
                selView
            };
            
            CurrentView = selView;
            
            //CurrentView.UpdateView();
            InitViews();
            
            CurrentView.UpdateView();
        }
       
        #endregion Constructors
        
        #region Public functions
        
        /// <summary>
        /// Switch to next sub-exercise in current parent exercise
        /// </summary>
        public void NextSubExercise()
        {
            // Disable main display on first navigation to the right
            if (CurrentView.ShowMainDisplay && GlobalDataModel.CurrentSubExerciseIndex == 0)
            {
                CurrentView.ShowMainDisplay = false;
                CurrentView.UpdateView();
                return;
            }
   
            // Calculate results if navigating to next on last sub exercise
            if (GlobalDataModel.CurrentSubExerciseIndex == CurrentExercise.Datasets.Count - 1)
            {
                // Print result summary
                var sb = new StringBuilder("Exercise results:\n");
                var correctCount = 0;
                for (var i = 0; i < CurrentExercise.CorrectAnswers.Count; i++)
                {
                    var chosenAnswer = CurrentExercise.ChosenAnswers[i];
                    var correctAnswer = CurrentExercise.CorrectAnswers[i];
                    
                    sb.Append("Chosen: " + chosenAnswer + ", Correct: " + correctAnswer + "\n");

                    if (chosenAnswer == correctAnswer) ++correctCount;
                }
                
                sb.Append("Result: [" + correctCount + "/" + CurrentExercise.CorrectAnswers.Count + "] correct!");
            
                //Log.Debug(sb.ToString());
                Debug.Log(sb.ToString());
                
                return;
            }
            
            ++GlobalDataModel.CurrentSubExerciseIndex;
            CurrentView.UpdateView();
        }

        
        /// <summary>
        /// Switch to previous sub-exercise in current parent exercise
        /// </summary>
        public void PreviousSubExercise()
        {
            if (GlobalDataModel.CurrentSubExerciseIndex == 0)
            {
                CurrentView.ShowMainDisplay = true;
                CurrentView.UpdateView();
                return;
            }
            
            --GlobalDataModel.CurrentSubExerciseIndex;
            CurrentView.UpdateView();
        }

        /// <summary>
        /// Save selection based on user choice
        /// </summary>
        /// <param name="choice">Chosen value</param>
        public void SetSelection(int choice)
        {
            //Debug.Log("choice set: " + choice);
            SelectionIndices[GlobalDataModel.CurrentSubExerciseIndex] = choice;
            CurrentExercise.ChosenAnswers[GlobalDataModel.CurrentSubExerciseIndex] = choice;
        }

        /// <summary>
        /// Set visibility on current exercise view
        /// </summary>
        /// <param name="value">View visible?</param>
        public override void SetViewVisibility(bool value)
        {
            base.SetViewVisibility(value);

            if (value)
            {
                CurrentView.UpdateView();
            }
        }
        
        #endregion Public functions
    }
}