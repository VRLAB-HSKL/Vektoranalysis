using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolylineView : MonoBehaviour
{
    public TubeMesh _mesh;

    private int _numSamplingPoints;

    [Range(20, 200)]
    public int NumberOfSamplingPoints = 20;


    public void OnValidate()
    {
        //_mesh.GenerateFieldMesh(NumberOfSamplingPoints);
    }

    // Start is called before the first frame update
    public void Start()
    {
        _mesh.GenerateFieldMesh(NumberOfSamplingPoints);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
