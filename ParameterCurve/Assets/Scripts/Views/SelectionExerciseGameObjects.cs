using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SelectionExerciseGameObjects : MonoBehaviour
{
    public Vector3 PillarOffset = Vector3.right;
    public Vector3 CurveOffset = Vector3.zero;
    public float ScalingFactor = 0.25f;

    public TextMeshProUGUI ExerciseTitle;
    public TextMeshProUGUI SubExerciseIdentifier;
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI MiddleDisplayText;

    public GameObject SelectionParent;
    public GameObject MainDisplayParent;
        
    public Material CurveLineMat;
}
