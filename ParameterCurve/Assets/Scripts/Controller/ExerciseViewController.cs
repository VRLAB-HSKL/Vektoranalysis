using System.Collections.Generic;
using System.Linq;
using Import.NewInitFile;
using UnityEngine;
using Views;


namespace Controller
{
    /// <summary>
    /// Controller for in-game views of exercise related data structures
    /// </summary>
    public class ExerciseViewController : AbstractViewController
    {
        /// <summary>
        /// Model of the controller, representing a selection exercise
        /// </summary>
        public SelectionExercise CurrentExercise
        {
            get
            {
                if (GlobalData.SelectionExercises.Any())
                {
                    return GlobalData.SelectionExercises[GlobalData.CurrentExerciseIndex];    
                }

                return null;
            }
        }
        
        /// <summary>
        /// Current exercise index for multiple exercises
        /// </summary>
        //public int currentExerciseIndex;
        
        /// <summary>
        /// Chosen selections in selection exercise, all -1 per default for non-choice
        /// </summary>
        public List<int> selectionIndices = new List<int>();

        public ExerciseViewController(Transform root, SelectionExerciseGameObjects selObjs, GameObject pillarPrefab,
            CurveControllerTye type) : base(root)
        {
            //Debug.Log("globalDataSelectionExercises[0] is null: " + (GlobalData.SelectionExercises[0] is null));
            //exercise = ;
            //(CurrentView as SelectionExerciseCompoundView).CurrentExerciseData = exercise.Datasets[0];


            // if (CurrentExercise is null)
            // {
            //     Debug.Log("CurrentExercise null");
            // }
            //
            // if (CurrentExercise.CorrectAnswers is null)
            // {
            //     Debug.Log("CurrentExercise.CorrectAnswers null");
            // }

            Debug.Log("test123");
            
            if (CurrentExercise != null)
            {
                
                
                // Initialize all answers as 'none given'
                for (int i = 0; i < CurrentExercise.CorrectAnswers.Count; i++)
                {
                    selectionIndices.Add(-1);
                }
            
                var selView = new SelectionExerciseCompoundView(selObjs, pillarPrefab, root, 
                    CurrentExercise.Datasets[0], type);
            
                selView.CurrentTitle = CurrentExercise.Title;
                selView.CurrentDescription = CurrentExercise.Description;
                _views = new List<AbstractView>()
                {
                    selView
                };

                CurrentView = selView;
            
            
            
                //CurrentView.UpdateView();
                InitViews();
            
                CurrentView.UpdateView();
                //Debug.Log("Finished exercise view controller constructor");    
            }
            
        }
       
        
        public void NextSubExercise()
        {
            // Debug.Log("showMainDisplay: " + (CurrentView as SelectionExerciseCompoundView).showMainDisplay);
            // Debug.Log("currentExerciseIndex: " + currentExerciseIndex);

            if (CurrentExercise is null)
                return;
            
            
            if ((CurrentView as SelectionExerciseCompoundView).showMainDisplay && GlobalData.CurrentSubExerciseIndex == 0)
            {
                // showMainDisplay = false;
                (CurrentView as SelectionExerciseCompoundView).showMainDisplay = false;
                CurrentView.UpdateView();
                return;
            }

            if (GlobalData.CurrentSubExerciseIndex == CurrentExercise.Datasets.Count - 1)
            {
                int correctCount = 0;
                for (int i = 0; i < CurrentExercise.CorrectAnswers.Count; i++)
                {
                    int chosenAnswer = CurrentExercise.ChosenAnswers[i];
                    int correctAnswer = CurrentExercise.CorrectAnswers[i];
                    
                    Debug.Log("Chosen: " + chosenAnswer + ", Correct: " + correctAnswer);

                    if (chosenAnswer == correctAnswer) ++correctCount;
                }
                
                Debug.Log("Result: [" + correctCount + "/" + CurrentExercise.CorrectAnswers.Count + "] correct!");
            
                return;
            }
            
            ++GlobalData.CurrentSubExerciseIndex;
            
            //(CurrentView as SelectionExerciseCompoundView).CurrentExerciseData =
            //    exercise.Datasets[currentExerciseIndex];
            //(CurrentView as SelectionExerciseCompoundView).CurrentExerciseIndex = currentExerciseIndex;
            
            CurrentView.UpdateView();
        }

        public void PreviousSubExercise()
        {
            if (CurrentExercise is null)
                return;
            
            if (GlobalData.CurrentSubExerciseIndex == 0)
            {
                //showMainDisplay = true;
                (CurrentView as SelectionExerciseCompoundView).showMainDisplay = true;
                CurrentView.UpdateView();
                return;
            }
            
            --GlobalData.CurrentSubExerciseIndex;
            // (CurrentView as SelectionExerciseCompoundView).CurrentExerciseData =
            //     exercise.Datasets[currentExerciseIndex];
            // (CurrentView as SelectionExerciseCompoundView).CurrentExerciseIndex = currentExerciseIndex;
            CurrentView.UpdateView();
        }

        public void SetSelection(int choice)
        {
            //selectionIndex = choice;
            selectionIndices[GlobalData.CurrentSubExerciseIndex] = choice;
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