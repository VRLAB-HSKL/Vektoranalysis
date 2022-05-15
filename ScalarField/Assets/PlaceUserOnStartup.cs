using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceUserOnStartup : MonoBehaviour
{
    public MeshFilter field;
    
    // Start is called before the first frame update
    void Start()
    {
        var user = GameObject.Find("ViveRig");

        if (user is null) return;

        // var points = new List<Vector3>();
        // field.mesh.GetVertices(points);
        //
        // Debug.Log("Attempting to place at index: " + GlobalDataModel.EstimatedIndex + "/" + points.Count);
        // var position = field.transform.TransformPoint(points[GlobalDataModel.EstimatedIndex]);
        // user.transform.position = position;



        var initVector = GlobalDataModel.ClosestPointOnMesh;//new Vector3(0.5f, 0f, -0.5f);
            //GlobalDataModel.ClosestPointOnMesh; //new Vector3(1f, 0f, 1f); 
        //GlobalDataModel.MainMeshScalingVector;
        // var scaleVector = new Vector3(1f /initScaleVector.x, 1f /initScaleVector.y, 1f /initScaleVector.z);
        // var finalPoint = Vector3.Scale(GlobalDataModel.ClosestPointOnMesh, scaleVector);
        //     
        // Debug.Log("initScaleVector: " + initScaleVector + ", scaleVector: " + scaleVector +
        //     "\nPlacing at final point: " + finalPoint);

        //var offsetVector = new Vector3(initScaleVector.x - 0.5f, initScaleVector.y, initScaleVector.z - 0.5f);

        // Flip sign on x coordinate
        initVector = new Vector3(-initVector.x, initVector.y, initVector.z);

        //initVector = new Vector3(initVector.x + 0.5f, initVector.y, initVector.z + 0.5f);
        
        //var offsetVector = initScaleVector;

        var boundsSize = field.mesh.bounds.size;
        var finalPoint = initVector;
        finalPoint = new Vector3(
            finalPoint.z * boundsSize.x, 
            finalPoint.y,
            finalPoint.x * boundsSize.z);
        
        
        //var finalPoint = new Vector3(offsetVector.x * 10f, offsetVector.y * 10f, offsetVector.z);

        
        
        Debug.Log("initScaleVector: " + initVector + //", offsetVector: " + offsetVector +
             "\nPlacing at final point: " + finalPoint);
        
        Debug.Log("MeshBounds x z: " + field.mesh.bounds.extents.x + " " + field.mesh.bounds.extents.z);



        var finalY = 2f;
        
        
        // Shoot down and up in hopes of hitting the mesh and positioning the player above the hit point
        if (Physics.Raycast(new Ray(finalPoint, Vector3.down), out RaycastHit hitDown))
        {
            finalY += hitDown.point.y; 
            //finalPoint.Set(finalPoint.x, hitDown.point.y + verticalOffset, finalPoint.z); 
        }
        else if (Physics.Raycast(new Ray(finalPoint, Vector3.up), out RaycastHit hitUp))
        {
            finalY += hitUp.point.y;
        }
        
        finalPoint = new Vector3(finalPoint.x, finalY, finalPoint.z); 
        
        //field.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        //field.GetComponent<MeshFilter>().mesh.RecalculateTangents();
        
        user.transform.position = finalPoint;

    }
    
}
