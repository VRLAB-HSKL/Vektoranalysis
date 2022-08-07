using System.Collections.Generic;

namespace Model
{
    public abstract class AbstractExercise
    {
        #region Public members
        
        /// <summary>
        /// Exercise title
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// Exercise description
        /// </summary>
        public string Description { get; protected set; }
        
        /// <summary>
        /// Amount of sub-exercises in this exercise
        /// </summary>
        public int NumberOfSubExercises { get; protected set; }

        /// <summary>
        /// Exercise datasets
        /// </summary>
        public List<AbstractExerciseDataset> Datasets { get; protected set; }
    
        /// <summary>
        /// Sub-exercise answers
        /// </summary>
        public List<AbstractExerciseAnswer> CorrectAnswers { get; protected set; }
        
        /// <summary>
        /// Answers chosen by the user
        /// </summary>
        public List<AbstractExerciseAnswer> ChosenAnswers { get; protected set; }
        
        /// <summary>
        /// Answers chosen by the user in directly previous attempt
        /// </summary>
        public List<AbstractExerciseAnswer> PreviousAnswers { get; protected set; }

        /// <summary>
        /// Number of correct answers in directly previous attempt
        /// </summary>
        public int previousScore { get; set; }

        /// <summary>
        /// Number of correct answers in current attempt
        /// </summary>
        public int currentScore { get; set; }

        /// <summary>
        /// Number of times the exercise has been attempted
        /// </summary>
        public int numAttempts { get; set; }

        #endregion Public members

        #region Constructors

        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="title">Title of exercise</param>
        /// <param name="description">Description of exercise</param>
        /// <param name="exercisePointDatasets">Datasets of sub-exercises</param>
        /// <param name="correctAnswers">Correct answers of sub-exercises</param>
        protected AbstractExercise(string title, string description)
        {
            Title = title;
            Description = description;
            //NumberOfSubExercises = CorrectAnswers.Count;

            // Datasets = exercisePointDatasets;
            // CorrectAnswers = correctAnswers;
            // ChosenAnswers = new List<int>();
            // PreviousAnswers = new List<int>();
            
            previousScore = -1;
            // for(int i = 0; i < NumberOfSubExercises; i++)
            // {
            //     ChosenAnswers.Add(-1);
            //     PreviousAnswers.Add(-1);
            // }        
        }


        #endregion Constructors

    }

    public abstract class AbstractExerciseDataset
    {
        public string HeaderText;

        
        protected AbstractExerciseDataset(string headerText)
        {
            HeaderText = headerText;
        }

        public abstract List<CurveInformationDataset> GetCurveData();
        
    }

    public abstract class AbstractExerciseAnswer
    {
        public bool Equals(AbstractExerciseAnswer obj)
        {
            var values01 = GetValues();
            var values02 = obj.GetValues();

            var v1Count = values01.Count;
            var v2Count = values02.Count;

            if (v1Count != v2Count) return false;

            for (var i = 0; i < v1Count; i++)
            {
                var valuesEqual = values01[i].Equals(values02[i]);
                if (!valuesEqual) return false;
            }

            return true;
        }

        public abstract List<float> GetValues();
        public abstract void SetValues(List<float> values);
        public abstract bool IsValid();
        
    }
}