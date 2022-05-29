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

        var simpleMesh = field.GetComponent<SimpleProceduralMesh>();
        GlobalDataModel.MainMeshScalingVector = simpleMesh.ScalingVector;
        
        StartCoroutine(LoadSceneAsync("Detail"));

    }

    IEnumerator LoadSceneAsync(string name)
    {
        var asyncOp = SceneManager.LoadSceneAsync(name);
        
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }

}
