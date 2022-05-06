using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUserOnStartup : MonoBehaviour
{
    public MeshFilter field;
    
    // Start is called before the first frame update
    void Start()
    {
        var user = GameObject.Find("ViveRig");

        if (user is null) return;

        var points = new List<Vector3>();
        field.mesh.GetVertices(points);

        Debug.Log("Attempting to place at index: " + GlobalDataModel.EstimatedIndex + "/" + points.Count);
        var position = field.transform.TransformPoint(points[GlobalDataModel.EstimatedIndex]);
        user.transform.position = position;



        user.transform.position = new Vector3(0f, 2f, 0f);
    }
}
