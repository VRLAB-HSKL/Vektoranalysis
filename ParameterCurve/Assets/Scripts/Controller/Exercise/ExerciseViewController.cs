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
        /// <summary>
        /// Model of the controller, representing a selection exercise
        /// </summary>
        private static SelectionExercise CurrentExercise => 
            GlobalDataModel.SelectionExercises.Any() 
                ? GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex] : null;

        /// <summary>
        /// Chosen selections in selection exercise, all -1 per default for non-choice
        /// </summary>
        public List<int> SelectionIndices = new List<int>();

        
        #region Private members

        /// <summary>
        /// Static log4net logger instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractExerciseViewController));
        
        #endregion Private members
        
        
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
       
        public void NextSubExercise()
        {
            // Disable main display on first navigation to the right
            if (CurrentView.showMainDisplay && GlobalDataModel.CurrentSubExerciseIndex == 0)
            {
                CurrentView.showMainDisplay = false;
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

        public void PreviousSubExercise()
        {
            if (GlobalDataModel.CurrentSubExerciseIndex == 0)
            {
                CurrentView.showMainDisplay = true;
                CurrentView.UpdateView();
                return;
            }
            
            --GlobalDataModel.CurrentSubExerciseIndex;
            CurrentView.UpdateView();
        }

        public void SetSelection(int choice)
        {
            Debug.Log("choice set: " + choice);
            SelectionIndices[GlobalDataModel.CurrentSubExerciseIndex] = choice;
            CurrentExercise.ChosenAnswers[GlobalDataModel.CurrentSubExerciseIndex] = choice;
        }

        public override void SetViewVisibility(bool value)
        {
            base.SetViewVisibility(value);

            if (value)
            {
                CurrentView.UpdateView();
            }
        }

    }
}