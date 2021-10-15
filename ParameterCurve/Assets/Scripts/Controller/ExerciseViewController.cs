using System.Collections.Generic;
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
        public SelectionExercise exercise { get; set; }
        
        /// <summary>
        /// Current exercise index for multiple exercises
        /// </summary>
        public int currentExerciseIndex;
        
        /// <summary>
        /// Chosen selections in selection exercise, all -1 per default for non-choice
        /// </summary>
        public List<int> selectionIndices = new List<int>();

        public ExerciseViewController(Transform root, SelectionExerciseGameObjects selObjs, GameObject pillarPrefab) : base(root)
        {
            //Debug.Log("globalDataSelectionExercises[0] is null: " + (GlobalData.SelectionExercises[0] is null));
            exercise = GlobalData.SelectionExercises[0];
            //(CurrentView as SelectionExerciseCompoundView).CurrentExerciseData = exercise.Datasets[0];
            
            
            
            // Initialize all answers as 'none given'
            for (int i = 0; i < exercise.CorrectAnswers.Count; i++)
            {
                selectionIndices.Add(-1);
            }

            var selView = new SelectionExerciseCompoundView(selObjs, pillarPrefab, root, exercise.Datasets[0]);
            selView.CurrentTitle = exercise.Title;
            selView.CurrentDescription = exercise.Description;
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
       
        
        public void NextSubExercise()
        {
            // Debug.Log("showMainDisplay: " + (CurrentView as SelectionExerciseCompoundView).showMainDisplay);
            // Debug.Log("currentExerciseIndex: " + currentExerciseIndex);
            
            if ((CurrentView as SelectionExerciseCompoundView).showMainDisplay && currentExerciseIndex == 0)
            {
                // showMainDisplay = false;
                (CurrentView as SelectionExerciseCompoundView).showMainDisplay = false;
                CurrentView.UpdateView();
                return;
            }

            if (currentExerciseIndex == exercise.Datasets.Count - 1)
            {
                int correctCount = 0;
                for (int i = 0; i < exercise.CorrectAnswers.Count; i++)
                {
                    int chosenAnswer = exercise.ChosenAnswers[i];
                    int correctAnswer = exercise.CorrectAnswers[i];
                    
                    Debug.Log("Chosen: " + chosenAnswer + ", Correct: " + correctAnswer);

                    if (chosenAnswer == correctAnswer) ++correctCount;
                }
                
                Debug.Log("Result: [" + correctCount + "/" + exercise.CorrectAnswers.Count + "] correct!");
            
                return;
            }
            
            ++currentExerciseIndex;
            
            (CurrentView as SelectionExerciseCompoundView).CurrentExerciseData =
                exercise.Datasets[currentExerciseIndex];
            (CurrentView as SelectionExerciseCompoundView).CurrentExerciseIndex = currentExerciseIndex;
            
            CurrentView.UpdateView();
        }

        public void PreviousSubExercise()
        {
            if (currentExerciseIndex == 0)
            {
                //showMainDisplay = true;
                (CurrentView as SelectionExerciseCompoundView).showMainDisplay = true;
                CurrentView.UpdateView();
                return;
            }
            
            --currentExerciseIndex;
            (CurrentView as SelectionExerciseCompoundView).CurrentExerciseData =
                exercise.Datasets[currentExerciseIndex];
            (CurrentView as SelectionExerciseCompoundView).CurrentExerciseIndex = currentExerciseIndex;
            CurrentView.UpdateView();
        }

        public void SetSelection(int choice)
        {
            //selectionIndex = choice;
            selectionIndices[currentExerciseIndex] = choice;
            exercise.ChosenAnswers[currentExerciseIndex] = choice;
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