using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Controller
{
    public class ExerciseViewController : AbstractViewController
    {
        public SelectionExercise exercise;
        public int currentExerciseIndex;
        public List<int> selectionIndices;

        private bool showMainDisplay = true;
        
        public ExerciseViewController(Transform root, SelectionExerciseGameObjects selObjs, GameObject pillarPrefab) : base(root)
        {
            InitExercises();

            var selView = new SelectionExerciseCompoundView(selObjs, pillarPrefab, root);
            selView.CurrentTitle = exercise.Title;
            selView.CurrentTitle = exercise.Description;
            _views = new List<IView>()
            {
                selView
            };
        }
        
        private void InitExercises()
        {
            exercise = GlobalData.SelectionExercises[0];
        }
        
        public void NextSubExercise()
        {
            if (showMainDisplay && currentExerciseIndex == 0)
            {
                showMainDisplay = false;
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
            exercise.ChosenAnswers[currentExerciseIndex] = choice;
        }

    }
}