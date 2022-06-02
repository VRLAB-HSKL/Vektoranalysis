using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CreateIsolines : MonoBehaviour
{
    public float epsilon = 0.01f;
    
    private List<float> IsoValues = new List<float>();

    private List<List<Vector3>> isolinePointArrays;
    
    // Start is called before the first frame update
    void Start()
    {
        IsoValues = new List<float>()
        {
            0f, 1f, 2f
        };

        isolinePointArrays = new List<List<Vector3>>();
        
        for (var i = 0; i < IsoValues.Count; i++)
        {
            var isoValue = IsoValues[i];
            var pointList = new List<Vector3>();
            for(var j = 0; j < GlobalDataModel.CurrentField.rawPoints.Count; ++j)
            {
                var point = GlobalDataModel.CurrentField.rawPoints[j];
                if (Mathf.Abs(point.z - isoValue) <= epsilon)
                {
                    Debug.Log("Isoline hit !\n" +
                              "iso value: " + isoValue + "\n" +
                              "point z: " + point.z + "\n" + 
                              "display point: " + GlobalDataModel.CurrentField.displayPoints[j]
                              );
                    pointList.Add(GlobalDataModel.CurrentField.displayPoints[j]);
                }
            }
            
            isolinePointArrays.Add(pointList);
        }

        for (var i = 0; i < isolinePointArrays.Count; i++)
        {
            var pointList = isolinePointArrays[i];
            if (pointList.Count == 0) continue;
            
            var go = new GameObject("IsoLine_" + IsoValues[i]);
            go.transform.SetParent(transform);
            
            var lr = go.AddComponent<LineRenderer>();

            // ToDo: Make this dynamic based on loaded scene
            // Scale points for main scene mesh 
            var newPointList = new List<Vector3>();
            for (var j = 0; j < pointList.Count; j++)
            {
                var p = pointList[j];
                var newp = Vector3.Scale(p, GlobalDataModel.MainMeshScalingVector);
                newPointList.Add(newp);
            }

            lr.positionCount = pointList.Count;
            lr.SetPositions(newPointList.ToArray());
            lr.material.color = Color.green;
            lr.widthMultiplier = 0.0125f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
