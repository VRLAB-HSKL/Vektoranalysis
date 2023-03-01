using TMPro;
using UnityEngine;


namespace ParamCurve.Scripts.Views
{
    public class SelectionExerciseGameObjects : MonoBehaviour
    {
        public GameObject SelectionRoot;
        public GameObject TangentNormalRoot;
    
        public Vector3 PillarOffset { get; set; } = Vector3.right;
        public Vector3 CurveOffset { get; set; } = Vector3.zero;
        public float ScalingFactor { get; set; } = 1f;

    
    
        public TextMeshProUGUI ExerciseTitle;
        public TextMeshProUGUI SubExerciseIdentifier;
        public TextMeshProUGUI HeaderText;
        public TextMeshProUGUI MiddleDisplayText;

        //results panel
        public TextMeshProUGUI PreviousAnswerText;
        public TextMeshProUGUI ChosenAnswerText;
        public TextMeshProUGUI CorrectIncorrectText;
        public TextMeshProUGUI OverallResultText;

        public GameObject SelectionParent;
        public GameObject MainDisplayParent;
        public GameObject ConfirmationDisplayParent;
        public GameObject ResultsDisplayParent;
        
        public Material CurveLineMat;

        public GameObject leftPillar;
        public GameObject middlePillar;
        public GameObject rightPillar;

        public GameObject TangentNormalPillar;

        public GameObject RetryButton;
        public GameObject ResetButton;
    }
}
