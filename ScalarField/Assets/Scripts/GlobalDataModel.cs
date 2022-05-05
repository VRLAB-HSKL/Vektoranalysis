
using UnityEngine;

public static class GlobalDataModel
{
    public static int EstimatedIndex = 0;
    
    /// <summary>
    /// Global sample count to make mesh indexing applicable to every mesh
    /// </summary>
    public static int NumberOfSamples = 200;

    public static Vector3 ClosestPointOnMesh = Vector3.zero;
}
