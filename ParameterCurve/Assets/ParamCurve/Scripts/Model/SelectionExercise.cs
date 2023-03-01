using System.Collections.Generic;
using System.Linq;

namespace ParamCurve.Scripts.Model
{
    /// <summary>
    /// Exercise based on selection execution
    /// </summary>
    public class SelectionExercise : AbstractExercise
    {
        #region Public members
        

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
            string title, string description, List<SelectionExerciseDataset> exercisePointDatasets, 
            List<SelectionExerciseAnswer> correctAnswers) : base(title, description)
        {
            NumberOfSubExercises = correctAnswers.Count;

            Datasets = new List<AbstractExerciseDataset>();
            foreach (var exp in exercisePointDatasets)
                Datasets.Add(exp);
            
            CorrectAnswers = new List<AbstractExerciseAnswer>();
            foreach (var ca in correctAnswers)
                CorrectAnswers.Add(ca);
            
            ChosenAnswers = new List<AbstractExerciseAnswer>();
            PreviousAnswers = new List<AbstractExerciseAnswer>();
            for(var i = 0; i < NumberOfSubExercises; i++)
            {
                ChosenAnswers.Add(new SelectionExerciseAnswer(-1));
                PreviousAnswers.Add(new SelectionExerciseAnswer(-1));
            }        
        }
        
        #endregion Constructors

    }

    /// <summary>
    /// Dataset model class for sub-exercise data
    /// </summary>
    public class SelectionExerciseDataset : AbstractExerciseDataset
    {
        #region Public members
        
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
        public SelectionExerciseDataset(string headerText, CurveInformationDataset leftDataset, 
            CurveInformationDataset middleDataset, CurveInformationDataset rightDataset) : base(headerText)
        {
            HeaderText = headerText;
            LeftDataset = leftDataset;
            MiddleDataset = middleDataset;
            RightDataset = rightDataset;
        }
        
        #endregion Constructors

        public override List<CurveInformationDataset> GetCurveData()
        {
            return new List<CurveInformationDataset>()
            {
                LeftDataset,
                MiddleDataset,
                RightDataset
            };
        }
    }

    public class SelectionExerciseAnswer : AbstractExerciseAnswer
    {
        public int PillarIndex;

        public SelectionExerciseAnswer()
        {
            PillarIndex = -1;
        }
        
        public SelectionExerciseAnswer(int pillarIndex)
        {
            PillarIndex = pillarIndex;
        }


        public override List<float> GetValues()
        {
            return new List<float>
            {
                PillarIndex
            };
        }

        public override void SetValues(List<float> values)
        {
            if (values.Any())
            {
                PillarIndex = (int) values[0];
            }
        }

        public override bool IsValid()
        {
            return PillarIndex != -1;
        }
    }
}