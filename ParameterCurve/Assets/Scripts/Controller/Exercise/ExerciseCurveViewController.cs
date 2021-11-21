using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Curve;
using log4net;
using UnityEngine;
using Views;

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
            GlobalData.SelectionExercises.Any() ? GlobalData.SelectionExercises[GlobalData.CurrentExerciseIndex] : null;

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

            // _views = new List<AbstractView>()
            // {
            //     selView
            // };
            //
            // CurrentView = selView;
            
            
            
            //CurrentView.UpdateView();
            InitViews();
            
            CurrentView.UpdateView();
        }
       
        public void NextSubExercise()
        {
            if (CurrentExercise is null)
                return;

            // Calculate results if navigating to next on last sub exercise
            if (GlobalData.CurrentSubExerciseIndex == CurrentExercise.Datasets.Count - 1)
            {
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
            
                Log.Debug(sb.ToString());
                
                return;
            }
            
            ++GlobalData.CurrentSubExerciseIndex;
            CurrentView.UpdateView();
        }

        public void PreviousSubExercise()
        {
            if (CurrentExercise is null)
                return;
            
            if (GlobalData.CurrentSubExerciseIndex == 0)
            {
                CurrentView.UpdateView();
                return;
            }
            
            --GlobalData.CurrentSubExerciseIndex;
            CurrentView.UpdateView();
        }

        public void SetSelection(int choice)
        {
            SelectionIndices[GlobalData.CurrentSubExerciseIndex] = choice;
            CurrentExercise.ChosenAnswers[GlobalData.CurrentSubExerciseIndex] = choice;
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