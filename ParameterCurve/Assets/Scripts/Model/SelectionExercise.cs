using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Exercise based on selection execution
    /// </summary>
    public class SelectionExercise
    {
        #region Public members
        
        /// <summary>
        /// Exercise title
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// Exercise description
        /// </summary>
        public string Description { get; }
        
        /// <summary>
        /// Amount of sub-exercises in this exercise
        /// </summary>
        public int NumberOfSubExercises { get; }

        /// <summary>
        /// Exercise datasets
        /// </summary>
        public List<ExercisePointDataset> Datasets { get; }
    
        /// <summary>
        /// Sub-exercise answers
        /// </summary>
        public List<int> CorrectAnswers { get; }
        
        /// <summary>
        /// Answers chosen by the user
        /// </summary>
        public List<int> ChosenAnswers { get; }

        #endregion Public members
        
        #region Constructors

        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="title">Title of exercise</param>
        /// <param name="description">Description of exercise</param>
        /// <param name="exercisePointDatasets">Datasets of sub-exercises</param>
        /// <param name="correctAnswers">Correct answers of sub-exercises</param>
        public SelectionExercise(
            string title, string description, List<ExercisePointDataset> exercisePointDatasets, 
            List<int> correctAnswers)
        {
            Title = title;
            Description = description;
            NumberOfSubExercises = correctAnswers.Count;

            Datasets = exercisePointDatasets;
            CorrectAnswers = correctAnswers;
            ChosenAnswers = new List<int>();
            for(int i = 0; i < NumberOfSubExercises; i++)
            {
                ChosenAnswers.Add(-1);
            }        
        }
        
        #endregion Constructors

    }

    /// <summary>
    /// Dataset model class for sub-exercise data
    /// </summary>
    public class ExercisePointDataset
    {
        #region Public members
        
        /// <summary>
        /// Header text of the dataset
        /// </summary>
        public string HeaderText { get; set; }
        
        /// <summary>
        /// Dataset of the left pillar
        /// </summary>
        public CurveInformationDataset LeftDataset { get; set; }
        
        /// <summary>
        /// Dataset of the middle pillar
        /// </summary>
        public CurveInformationDataset MiddleDataset { get; set; }
        
        /// <summary>
        /// Dataset of the right pillar
        /// </summary>
        public CurveInformationDataset RightDataset { get; set; }

        #endregion Public members
        
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="headerText">Header text</param>
        /// <param name="leftDataset">Left pillar dataset</param>
        /// <param name="middleDataset">Middle pillar dataset</param>
        /// <param name="rightDataset">Right pillar dataset</param>
        public ExercisePointDataset(string headerText, CurveInformationDataset leftDataset, 
            CurveInformationDataset middleDataset, CurveInformationDataset rightDataset)
        {
            HeaderText = headerText;
            LeftDataset = leftDataset;
            MiddleDataset = middleDataset;
            RightDataset = rightDataset;
        }
        
        #endregion Constructors
    }
}