using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class RotateTargetBasedOnPosition : MonoBehaviour
{
    public Transform Target;
    
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        var localPos = transform.localPosition;
        
        var clampedX = Mathf.Clamp(localPos.x, -0.45f, 0.45f);
        var mappedX = CalcUtil.MapRange(clampedX, -0.45f, 0.45f, -1f, 1f);
        
        //var clampedY = Mathf.Clamp(localPos.y, -0.45f, 0.45f);
        //var mappedX = CalcUtil.MapRange(clampedX, -0.45f, 0.45f, -1f, 1f);
        
        //var clampedX = Mathf.Clamp(localPos.x, -0.45f, 0.45f);
        //var mappedX = CalcUtil.MapRange(clampedX, -0.45f, 0.45f, -1f, 1f);

        var rotDegreeX = mappedX * 360f;

        if (rotDegreeX != 0f)
        {
            Debug.Log(
                "local Pos Val: " + localPos.x + "\n" +
                "clamped val: " + clampedX + "\n" +
                "mapped val: " + mappedX + "\n" +
                "rotDegree: " + rotDegreeX
            );    
        }

        // Reset rotation before rotating
        Target.transform.Rotate(0f, 0f, 0f);
        Target.transform.Rotate(rotDegreeX, 0f, 0f);
    }
}
