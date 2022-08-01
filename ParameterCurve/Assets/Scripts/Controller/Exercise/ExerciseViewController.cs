using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Curve;
using log4net;
using Model;
using UnityEngine;
using Views;
using Views.Exercise;
using System.IO;

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
        public List<int> SelectionIndices = new List<int>();

        public bool confirmedAnswers { get; set; } = false;

        #endregion Public members
        
        #region Private members

        /// <summary>
        /// Data model of the controller, representing a selection exercise
        /// </summary>
        private static AbstractExercise CurrentExercise => 
            GlobalDataModel.SelectionExercises.Any() 
                ? GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex] : null;
        
        /// <summary>
        /// Static log4net logger instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractExerciseViewController));

        private SelectionExerciseGameObjects _selObjects;

        //path to text file storing chosen answers for each attempt
        private string path = "Assets/Resources/exerciseresults.txt";

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

            _selObjects = selObjs;
            
            // Initialize all answers as 'none given'
            for (var i = 0; i < CurrentExercise.CorrectAnswers.Count; i++)
            {
                SelectionIndices.Add(-1);
            }
            
            var selView = new SelectionExerciseCompoundView(selObjs, pillarPrefab, root, 
                 type)
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

            //reset recorded answers on each new world state load
            File.WriteAllText(path, "");
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

            // Once user navigates right past the confirmation display, show results
            if (CurrentView.ShowConfirmationDisplay)
            {
                CurrentExercise.numAttempts++;

                // Print result summary and log it
                using (StreamWriter writer = new StreamWriter(path, append: true))
                {
                    var previousText = new StringBuilder("Previous Answer:\n");
                    var chosenText = new StringBuilder("Chosen Answer:\n");
                    var correctText = new StringBuilder("\n");
                    var resultText = new StringBuilder("");

                    CurrentExercise.currentScore = 0;
                    
                    //logging name and attempt number into text file
                    writer.WriteLine(CurrentExercise.Title);
                    writer.WriteLine("attempt" + CurrentExercise.numAttempts);

                    for (var i = 0; i < CurrentExercise.CorrectAnswers.Count; i++)
                    {
                        var previousAnswer = CurrentExercise.PreviousAnswers[i];
                        string previousAnsText = "";
                        var chosenAnswer = CurrentExercise.ChosenAnswers[i];
                        string chosenAnsText = "";
                        var correctAnswer = CurrentExercise.CorrectAnswers[i];
                        string correctAnsText = "";

                        if (CurrentExercise is SelectionExercise)
                        {
                            previousAnsText = ((SelectionExerciseAnswer)previousAnswer).PillarIndex.ToString();
                            chosenAnsText = ((SelectionExerciseAnswer)chosenAnswer).PillarIndex.ToString();
                            correctAnsText = ((SelectionExerciseAnswer)correctAnswer).PillarIndex.ToString();
                        } else if (CurrentExercise is TangentNormalExercise)
                        {
                            //Undecided what will be displayed for TangentNormal
                        }

                        string str = "";
                        //if first attempt, no previous to compare to
                        if (CurrentExercise.numAttempts == 1)
                        {
                            if (chosenAnswer.Equals(correctAnswer)) str = "<font=\"LiberationSans SDF\"><mark=#46d53a80>Correct</mark></font>";
                            else str = "<font=\"LiberationSans SDF\"><mark=#fa414180>Incorrect</mark></font>";
                            previousText.Append("none\n");
                        } else
                        {
                            
                            
                            if (!previousAnswer.Equals(correctAnswer) && !chosenAnswer.Equals(correctAnswer)) str = "<font=\"LiberationSans SDF\"><mark=#fa414180>Incorrect still</mark></font>";
                            else if (!previousAnswer.Equals(correctAnswer) && chosenAnswer.Equals(correctAnswer)) str = "<font=\"LiberationSans SDF\"><mark=#46d53aFF>Correct now</mark></font>";
                            else if (previousAnswer.Equals(correctAnswer) && !chosenAnswer.Equals(correctAnswer)) str = "<font=\"LiberationSans SDF\"><mark=#fa4141FF>Incorrect now</mark></font>";
                            else str = "<font=\"LiberationSans SDF\"><mark=#46d53a80>Correct still</mark></font>";

                            if(previousAnswer.IsValid())
                            {
                                previousText.Append(previousAnsText + "\n");
                            } else
                            {
                                previousText.Append("none\n");
                            }
                            
                        }

                        if(chosenAnswer.IsValid())
                        {
                            chosenText.Append(chosenAnsText + "\n");
                        } else
                        {
                            chosenText.Append("none\n");
                        }
                        
                        correctText.Append(str + "\n");

                        //log previous, chosen, and correct answers into text file
                        writer.WriteLine(previousAnsText + " " + chosenAnsText + " " + correctAnsText);

                        if (chosenAnswer.Equals(correctAnswer)) ++CurrentExercise.currentScore;
                    }

                    //if anything past first attempt, there is a previous score to show
                    if (CurrentExercise.numAttempts != 1)
                    {
                        resultText.Append("Previous result: [" + CurrentExercise.previousScore + "/" + CurrentExercise.CorrectAnswers.Count + "] correct!\n");
                    } else
                    {
                        resultText.Append("Previous result: none\n");
                    }

                    resultText.Append("Current result: [" + CurrentExercise.currentScore + "/" + CurrentExercise.CorrectAnswers.Count + "] correct!");
                    writer.WriteLine(CurrentExercise.currentScore + "/" + CurrentExercise.CorrectAnswers.Count);

                    _selObjects.PreviousAnswerText.text = previousText.ToString();
                    _selObjects.ChosenAnswerText.text = chosenText.ToString();
                    _selObjects.CorrectIncorrectText.text = correctText.ToString();
                    _selObjects.OverallResultText.text = resultText.ToString();
                }

                confirmedAnswers = true;
                CurrentView.ShowConfirmationDisplay = false;
                CurrentView.ShowResultsDisplay = true;
                CurrentView.UpdateView();
                return;
            }

            // If user is trying to submit answers for the first time, go to confirmation display
            if (!confirmedAnswers && GlobalDataModel.CurrentSubExerciseIndex == CurrentExercise.Datasets.Count - 1)
            {
                CurrentView.ShowConfirmationDisplay = true;
                CurrentView.UpdateView();
                return;
            }

            //cannot go further after getting results
            if (!confirmedAnswers)
            {
                ++GlobalDataModel.CurrentSubExerciseIndex;
                CurrentView.UpdateView();
            }

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

            //if going backwards from confirmation page to change answers
            if(CurrentView.ShowConfirmationDisplay)
            {
                CurrentView.ShowConfirmationDisplay = false;
                CurrentView.UpdateView();
                return;
            }

            //can go backwards from any of the exercises or on the confirmation, but cannot once results are displayed
            if (!confirmedAnswers)
            {
                --GlobalDataModel.CurrentSubExerciseIndex;
                CurrentView.UpdateView();
            }

        }

        /// <summary>
        /// Save selection based on user choice
        /// </summary>
        /// <param name="choice">Chosen value</param>
        public void SetSelection(int choice)
        {
            //Debug.Log("choice set: " + choice);
            SelectionIndices[GlobalDataModel.CurrentSubExerciseIndex] = choice;
            CurrentExercise.ChosenAnswers[GlobalDataModel.CurrentSubExerciseIndex].SetValues(new List<float> {choice});
        }

        /// <summary>
        /// Set visibility on current exercise view
        /// </summary>
        /// <param name="value">View visible?</param>
        public override void SetViewVisibility(bool value)
        {
            base.SetViewVisibility(value);
            
            //_selObjects.SelectionRoot.SetActive(value);
            
            if (value)
            {
                CurrentView.UpdateView();
            }
        }

        /// <summary>
        /// Called with retry button, exit button, or selecting current same exercise on menu wall
        /// Resets all selections, starts again from main display
        /// </summary>
        public void ResetCurrentExercise()
        {
            // set all answers as 'none given'
            for (int i = 0; i < CurrentExercise.NumberOfSubExercises; i++)
            {
                CurrentExercise.PreviousAnswers[i] = CurrentExercise.ChosenAnswers[i];
                CurrentExercise.previousScore = CurrentExercise.currentScore;
                SelectionIndices[i] = -1;
                CurrentExercise.ChosenAnswers[i].SetValues(new List<float> { -1 });
            }

            //start at main display again
            GlobalDataModel.CurrentSubExerciseIndex = 0;
            confirmedAnswers = false;
            CurrentView.ShowMainDisplay = true;
            CurrentView.ShowResultsDisplay = false;
            CurrentView.ShowConfirmationDisplay = false;
            CurrentView.UpdateView();
        }

        /// <summary>
        /// Called when a different exercise is selected on the wall
        /// Resets all selections, starts again from main display
        /// </summary>
        public void NewExercise()
        {
            //reset selections
            //necessary because not every exercise is the same size, so selectionIndices list should be re-initialized
            SelectionIndices = new List<int>();
            for (var i = 0; i < CurrentExercise.CorrectAnswers.Count; i++)
            {
                SelectionIndices.Add(-1);
            }

            ResetCurrentExercise();
        }

        #endregion Public functions
    }
}