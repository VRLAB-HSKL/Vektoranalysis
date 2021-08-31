using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSelectionView : MonoBehaviour
{
    public Vector3 PillarOffset = Vector3.right;
    public Vector3 CurveOffset = Vector3.zero;
    public float ScalingFactor = 0.25f;

    public Material CurveLineMat;

    AbstractCurveView leftView;
    AbstractCurveView middleView;
    AbstractCurveView rightView;

    private void Start()
    {
        InitLineRenders();

        leftView.UpdateView();
        middleView.UpdateView();
        rightView.UpdateView();
    }

    private void InitLineRenders()
    {
        GameObject leftPillar = Instantiate(new GameObject("LeftPillar"), transform);
        LineRenderer leftLR = leftPillar.AddComponent<LineRenderer>();
        leftLR.widthMultiplier = 0.05f;
        leftLR.material = CurveLineMat;

        GameObject middlePillar = Instantiate(new GameObject("MiddlePillar"), transform);
        LineRenderer middleLR = middlePillar.AddComponent<LineRenderer>();
        middleLR.widthMultiplier = 0.05f;
        middleLR.material = CurveLineMat;

        GameObject rightPillar = Instantiate(new GameObject("RightPillar"), transform);
        LineRenderer rightLR = rightPillar.AddComponent<LineRenderer>();
        rightLR.widthMultiplier = 0.05f;
        rightLR.material = CurveLineMat;

        leftPillar.transform.position -= PillarOffset;
        rightPillar.transform.position += PillarOffset;        

        leftView = new SimpleCurveView(leftLR, leftPillar.transform.position + CurveOffset, ScalingFactor);
        middleView = new SimpleCurveView(middleLR, middlePillar.transform.position + CurveOffset, ScalingFactor);
        rightView = new SimpleCurveView(rightLR, rightPillar.transform.position + CurveOffset, ScalingFactor);

    }


    
}
