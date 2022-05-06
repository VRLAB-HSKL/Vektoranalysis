using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VR.Scripts.Behaviours.Button;

public class TravelToWorldButtonBehaviour : AbstractButtonBehaviour
{
    public MeshFilter field;

    public float distanceEpsilon = 0.01f;
    
    protected override void HandleButtonEvent()
    {
        
        
        var travelObj = GameObject.Find("TravelTarget");

        if (travelObj is null)
        {
            Debug.Log("No location chosen yet!");
            return;
        }

        var travelPosition = GlobalDataModel.ClosestPointOnMesh;
        //var travelPosition =  travelObj.transform.position;

        List<Vector3> points = new List<Vector3>();
        int index = -1;
        field.mesh.GetVertices(points);
        
        //Debug.Log("pointCount: " + points.Count);

        var smalledDist = double.MaxValue;
        
        for(var i = 0; i < points.Count; i++)
        {
            var point = field.gameObject.transform.TransformPoint(points[i]);
            
            // Translate to gameobject origin
            //point += field.gameObject.transform.position;
            
            // Scale point based on gameobject global scaling
            //point = Vector3.Scale(point, field.gameObject.transform.lossyScale);

            // Only calculate distance based on 2d table dimension to circumvent mesh collider optimization
            var dist = Math.Sqrt(point.x * travelPosition.x + point.z * travelPosition.z); 
            //var dist = Vector3.Distance(point, travelPosition);

            if (dist < smalledDist) 
                smalledDist = dist;
            
            Debug.Log("initPoint: " + points[i] + ", transformedPoint: " + point + 
                      ", travelPos: " + travelPosition + ", distance: " + dist);
             
            if ( dist < distanceEpsilon )
            {
                index = i;
                //field.mesh.colors[i] = Color.magenta;
                break;
            }
        }

        if (index == -1)
        {
            Debug.Log("No matching index found! Smallest distance: " + smalledDist);
            return;
        }
        
        Debug.Log("Estimated index: " + index + ", smallest distance: " + smalledDist);

        //GlobalDataModel.EstimatedIndex = index;

        SceneManager.LoadScene("Detail");
        
        
        
    }

}
