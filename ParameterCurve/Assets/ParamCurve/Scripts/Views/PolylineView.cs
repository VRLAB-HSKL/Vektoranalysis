using UnityEngine;

public class PolylineView : MonoBehaviour
{
    /// <summary>
    /// Mesh controller class
    /// </summary>
    public TubeMesh _mesh;
    
    /// <summary>
    /// Curve sample count
    /// </summary>
    [Range(20, 200)]
    public int NumberOfSamplingPoints = 20;

    /// <summary>
    /// Curve sample count
    /// </summary>
    private int _numSamplingPoints;

    // Start is called before the first frame update
    public void Start()
    {
        _mesh.GenerateFieldMesh(NumberOfSamplingPoints);
    }
}
