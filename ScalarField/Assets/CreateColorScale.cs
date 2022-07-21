using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

public class CreateColorScale : MonoBehaviour
{
    public ScalarFieldManager ScalarFieldManager;
    public GameObject BoundingBox;
    
    private List<GameObject> _cubes;
    private Bounds _bounds;
    
    // Start is called before the first frame update
    void Start()
    {
        _bounds = BoundingBox.GetComponent<MeshRenderer>().bounds;
        _cubes = new List<GameObject>();
        
        SetCubes();
    }

    public void UpdateScale()
    {
        var colors = ScalarFieldManager.InitFile.DisplayFields[ScalarFieldManager.CurrentFieldIndex].Info.Colors;
        if (colors.Count == _cubes.Count)
        {
            for(var i = 0; i < _cubes.Count; i++)
            {
                var color = colors[i];
                var r = color[0] / 255f;
                var g = color[1] / 255f;
                var b = color[2] / 255f;
                _cubes[i].GetComponent<MeshRenderer>().material.color = new Color(r, g, b);
            }    
        }
        else
        {
            SetCubes();
        }
    }
    
    private void SetCubes()
    {
        var colors = ScalarFieldManager.InitFile.DisplayFields[ScalarFieldManager.CurrentFieldIndex].Info.Colors;
        var verticalLength = _bounds.size;
        var floor = _bounds.min.y;
        var verticalStep = verticalLength.y / (float)colors.Count;
        var verticalScaleVector = 1f / (float) colors.Count;

        foreach (var cube in _cubes)
        {
            Destroy(cube);
        }

        _cubes.Clear();
        for (var i = 0; i < colors.Count; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var mr = cube.GetComponent<MeshRenderer>();
            var mf = cube.GetComponent<MeshFilter>();

            cube.name = "Cube" + i;
            
            var mappedVertices = CalcUtility.MapDisplayVectors(mf.mesh.vertices.ToList(), _bounds, transform);
            mf.mesh.SetVertices(mappedVertices);
            
            cube.transform.parent = transform;
            
            // Scale cube in y direction to fit all cubes inside the scale
            // and slightly in x and z direction to prevent buffer fighting
            cube.transform.localScale = new Vector3(0.98f, verticalScaleVector, 0.98f);
            
            var y = floor + (i * verticalStep) + verticalStep * 0.5f;
            cube.transform.position = new Vector3(_bounds.center.x, y, _bounds.center.z);
            
            var currColor = colors[i];
            var r = currColor[0] / 255f;
            var g = currColor[1] / 255f;
            var b = currColor[2] / 255f;
            var color = new Color(r, g, b);

            mr.material.SetColor("_Color", color);
            
            _cubes.Add(cube);
        }
    }
    
}
