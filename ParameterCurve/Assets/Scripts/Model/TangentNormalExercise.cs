using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Exercise based on drawing tangent and normal lines
    /// </summary>
    public class TangentNormalExercise : AbstractExercise
    {
        #region Public members


        #endregion Public members

        #region Constructors

        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="title">Title of exercise</param>
        /// <param name="description">Description of exercise</param>
        /// <param name="tangentNormalDatasets">Datasets of sub-exercises</param>
        /// <param name="correctAnswers">Correct answers of sub-exercises</param>
        public TangentNormalExercise(
            string title, string description, List<TangentNormalExerciseDataset> tangentNormalDatasets,
            List<TangentNormalExerciseAnswer> correctAnswers) : base(title, description)
        {
            NumberOfSubExercises = correctAnswers.Count;

            Datasets = new List<AbstractExerciseDataset>();
            foreach (var tn in tangentNormalDatasets)
                Datasets.Add(tn);

            CorrectAnswers = new List<AbstractExerciseAnswer>();
            foreach (var ca in correctAnswers)
                CorrectAnswers.Add(ca);

            ChosenAnswers = new List<AbstractExerciseAnswer>();
            PreviousAnswers = new List<AbstractExerciseAnswer>();
            for(var i = 0; i < NumberOfSubExercises; i++)
            {
                //for each highlight point in each subexercise/curve
                List<float[]> listForCurve = new List<float[]>();
                for(var j = 0; j < ((TangentNormalExerciseDataset)Datasets[i]).HighlightPoints.Count; j++)
                {
                    listForCurve.Add(new float[] { -1, -1 });
                }
                ChosenAnswers.Add(new TangentNormalExerciseAnswer(listForCurve, listForCurve));
                PreviousAnswers.Add(new TangentNormalExerciseAnswer(listForCurve, listForCurve));
            }
        }

        #endregion Constructors

    }

    /// <summary>
    /// Dataset model class for sub-exercise data
    /// </summary>
    public class TangentNormalExerciseDataset : AbstractExerciseDataset
    {
        #region Public members

        /// <summary>
        /// Dataset of the main curve
        /// </summary>
        public CurveInformationDataset Curve { get; set; }

        /// <summary>
        /// Points that user will draw tangent and normal for
        /// </summary>
        public List<int> HighlightPoints { get; set; }

        #endregion Public members

        #region Constructors

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="headerText">Header text</param>
        /// <param name="curve">Main curve dataset</param>
        /// <param name="highlightPoints">Indices of points on curve where vectors will be drawn</param>
        public TangentNormalExerciseDataset(string headerText, CurveInformationDataset curve,
            List<int> highlightPoints) : base(headerText)
        {
            HeaderText = headerText;
            Curve = curve;
            HighlightPoints = highlightPoints;
        }

        #endregion Constructors

        public override List<CurveInformationDataset> GetCurveData()
        {
            return new List<CurveInformationDataset>() { Curve };
        }
    }

    public class TangentNormalExerciseAnswer : AbstractExerciseAnswer
    {
        //list of tangent/normal positions for each highlight point in sub exercise
        public List<float[]> TangentPos;
        public List<float[]> NormalPos;

        public TangentNormalExerciseAnswer(List<float[]> tangentPos, List<float[]> normalPos)
        {
            TangentPos = tangentPos;
            NormalPos = normalPos;
        }

        public override List<float> GetValues()
        {
            //format:
            //for each highlight point--> tangent x y, normal x y
            List<float> list = new List<float>();

            //for each highlight point
            for(var i = 0; i < TangentPos.Count; i++)
            {
                list.Add(TangentPos[i][0]); //tangent x
                list.Add(TangentPos[i][1]); //tangent y
                list.Add(NormalPos[i][0]);  //normal x
                list.Add(NormalPos[i][1]);  //normal y
            }

            return list;
        }

        public override void SetValues(List<float> values)
        {
            //if not enough values for a single highlight point or an incomplete set of tangent xy normal xy
            if (values.Count < 4 || values.Count % 4 != 0) return;

            //assuming order is tangent x y, normal x y
            for(var i = 0; i < values.Count; i+=4)  //for each highlight point (containing 4 values)
            {
                TangentPos[i/4][0] = values[i];
                TangentPos[i/4][1] = values[i + 1];
                NormalPos[i/4][0] = values[i + 2];
                NormalPos[i/4][1] = values[i + 3];
            }
        }

        public override bool IsValid()
        {
            bool allValid = true;

            //check for each highlight point
            for(var i = 0; i < TangentPos.Count; i++)
            {
                if (TangentPos[i][0] == -1 && TangentPos[i][1] == -1) { allValid = false; break; }
                if (NormalPos[i][0] == -1 && NormalPos[i][1] == -1) { allValid = false; break; }
            }

            return allValid;
        }
    }
}