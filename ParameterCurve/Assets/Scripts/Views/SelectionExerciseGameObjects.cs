using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SelectionExerciseGameObjects : MonoBehaviour
{
    public Vector3 PillarOffset { get; set; } = Vector3.right;
    public Vector3 CurveOffset { get; set; } = Vector3.zero;
    public float ScalingFactor { get; set; } = 1f;

    public TextMeshProUGUI ExerciseTitle;
    public TextMeshProUGUI SubExerciseIdentifier;
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI MiddleDisplayText;

    public GameObject SelectionParent;
    public GameObject MainDisplayParent;
        
    public Material CurveLineMat;

    public GameObject leftPillar;
    public GameObject middlePillar;
    public GameObject rightPillar;
}
