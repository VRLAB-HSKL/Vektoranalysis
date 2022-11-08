// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Model;
// using UnityEngine;
// using Utility;
//
// [RequireComponent(
//     typeof(MeshFilter),
//     typeof(MeshRenderer),
//     typeof(MeshCollider))]
// public class TubeMesh : MonoBehaviour
// {
//     # region Public members
//     
//     /// <summary>
//     /// Material used for the surface of the tube
//     /// </summary>
//     public Material tubeMat;
//
//     /// <summary>
//     /// Material used for the highlighting spheres on the tube
//     /// </summary>
//     public Material sphereMat;
//
//     /// <summary>
//     /// General scaling factor applied to the tube mesh
//     /// </summary>
//     public float tubeMeshScalingFactor = 1f;
//
//     #endregion Public members
//     
//     
//     #region Private members 
//     
//     
//     private float _sphereScalingFactor = 2f;
//
//     
//     /// <summary>
//     /// Radius of the final tube
//     /// </summary>
//     private float _radius = 0.1f;
//
//     /// <summary>
//     /// Number of point used to sample the flat circle around every curve point, making up the outside of the tube mesh
//     /// </summary>
//     private const int NumberOfCirclePoints = 8;
//
//     /// <summary>
//     /// Sub-mesh for the outer surface of the tube along the curve
//     /// </summary>
//     private Mesh _tubeMesh;
//     
//     /// <summary>
//     /// Sub-mesh for the bottom lid of the tube, facing outward
//     /// </summary>
//     private Mesh _bottomLidMesh;
//     
//     /// <summary>
//     /// Sub-mesh for the top lid of the tube, facing outward
//     /// </summary>
//     private Mesh _topLidMesh;
//
//     /// <summary>
//     /// Number of points making up the mesh
//     /// </summary>
//     private readonly List<Vector3> _tubePoints = new List<Vector3>();
//     
//     /// <summary>
//     /// Number of spheres highlighting the points on the curve
//     /// </summary>
//     private readonly List<GameObject> _spheres = new List<GameObject>();
//     
//     /// <summary>
//     /// Game object for the bottom lid mesh
//     /// </summary>
//     private GameObject _bottomLidGameObject;
//     
//     /// <summary>
//     /// Game object for the top lid mesh
//     /// </summary>
//     private GameObject _topLidGameObject;
//
//     /// <summary>
//     /// Calculated amount of degrees between the points along the circle surrounding each curve point
//     /// </summary>
//     private float _degreeStepSize;
//
//     /// <summary>
//     /// Custom number of sampling points for a curve. Can be used to visualize the difference
//     /// in differing sampling counts
//     /// </summary>
//     private int _numberOfSamplingPoints = -1;
//     
//     /// <summary>
//     /// Signals whether the curve points are visualized along the curve using small spheres
//     /// </summary>
//     private bool _visualizePoints = false;
//
//     /// <summary>
//     /// Cached mesh renderer component
//     /// </summary>
//     private MeshRenderer _meshRenderer;
//     
//     /// <summary>
//     /// Cached mesh filter component
//     /// </summary>
//     private MeshFilter _meshFilter;
//     
//     /// <summary>
//     /// Cached mesh collider component
//     /// </summary>
//     private MeshCollider _meshCollider;
//
//     /// <summary>
//     /// Curve scaling factor. Changed by <see cref="SetScalingFactor"/>
//     /// </summary>
//     private float _scalingFactor = 1f;
//     
//     private const float CircleDegree = 1f / 360f;
//     
//     #endregion Private members
//     
//     
//     #region Public functions
//     
//     private void Awake()
//     {
//         // Cache components
//         _meshCollider = GetComponent<MeshCollider>();
//         _meshFilter = GetComponent<MeshFilter>();
//         _meshRenderer = GetComponent<MeshRenderer>();
//         
//         // Set tube radius based on scaling factor
//         _radius = 0.1f;//0.05f * tubeMeshScalingFactor;
//         _sphereScalingFactor = 0.125f * tubeMeshScalingFactor;
//         
//     }
//
//     public void SetScalingFactor(float val)
//     {
//         _scalingFactor = val;
//         tubeMeshScalingFactor = _scalingFactor;
//         _radius = 0.1f; //Mathf.Clamp(0.05f * _scalingFactor, 0.1f, 0.1f);
//         _sphereScalingFactor = 0.125f * _scalingFactor;
//     }
//     
//     /// <summary>
//     /// Creates the mesh by calculation the topology
//     /// </summary>
//     public void GenerateFieldMesh()
//     {
//         CalculateMesh();
//     }
//     
//     /// <summary>
//     /// Creates the mesh using a given amount of points to sample the curve
//     /// </summary>
//     /// <param name="sampleCount">Sample count</param>
//     public void GenerateFieldMesh(int sampleCount)
//     {
//         _numberOfSamplingPoints = sampleCount;
//         CalculateMesh();
//     }
//
//     #endregion Public functions
//     
//     /// <summary>
//     /// Mesh calculation function
//     /// </summary>
//     private void CalculateMesh()
//     {
//         if(GlobalDataModel.DisplayCurveDatasets == null) return;
//         if(GlobalDataModel.DisplayCurveDatasets.Count == 0) return;
//
//         _tubePoints.Clear();
//         _degreeStepSize = 360f / NumberOfCirclePoints;
//
//         StartCoroutine(GenerateCurveMesh());
//
//     }
//     
//     /// <summary>
//     /// Generates the main surface around the curve points
//     /// </summary>
//     private IEnumerator GenerateCurveMesh()
//     {
//         var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
//         var polyline = curve.WorldPoints;
//
//         // When a sample count was given, sample curve
//         // ToDo: Refactor this into second view / subclass
//         if(_numberOfSamplingPoints != -1)
//         {
//             var div = polyline.Count / _numberOfSamplingPoints;
//             _degreeStepSize = _numberOfSamplingPoints;
//
//             var newPointList = new List<Vector3>();
//             for(var i = 0; i < polyline.Count; i++)
//             {
//                 // Only sample every nth point
//                 if(i % div == 0)
//                 {
//                     var p = polyline[i];
//                     newPointList.Add(p);
//                 }
//             }
//             
//             foreach(var sphere in _spheres)
//             {
//                 Destroy(sphere);
//             }
//
//             _spheres.Clear();
//             
//             foreach(var point in newPointList)
//             {
//                 var calcPoint = point;
//                 // if(!curve.Is3DCurve)
//                 // {
//                 //     calcPoint = new Vector3(point.x, point.y, 0f);
//                 // }
//                 
//                 calcPoint *= tubeMeshScalingFactor;
//                 var spherePoint = transform.position + calcPoint;
//
//                 if (_visualizePoints)
//                 {
//                     var sphere = DrawingUtility.DrawSphereScaled(spherePoint, transform, Color.black, 
//                         _sphereScalingFactor,
//                         sphereMat);
//
//                     sphere.GetComponent<MeshRenderer>().sharedMaterial = sphereMat;
//
//                     _spheres.Add(sphere);    
//                 }
//                 
//             }
//
//             polyline = newPointList;
//         }
//
//         var batchSize = 3;
//         
//         // Calculate surface mesh points
//         for (var i = 0; i < polyline.Count; i++)
//         {
//             //yield return null;
//             
//             // Get curve point and direction vectors
//             var centerPoint = polyline[i];
//             var tangent = curve.FresnetApparatuses[i].Tangent;
//             var biNormal = curve.FresnetApparatuses[i].Binormal;
//             
//             // On first point, generate tangent pointing to next point since
//             // we don't have any velocity at the beginning, i.e. the tangent is (0,0,0)
//             if (i == 0)
//             {
//                 tangent = (polyline[i + 1] - centerPoint).normalized;// * radius;
//                 biNormal = curve.FresnetApparatuses[i + 1].Binormal;
//             }
//             
//             //yield return null;
//             
//             // We need a vector to rotate around the curve point to generate a surface
//             // around the curve point. For this, we pick the bi-normal of the given
//             // curve point.
//             // ToDo: Fix edge case where bi-normal is parallel to tangent !
//             var perpendicularVec = biNormal;
//             var cpn = perpendicularVec.normalized * _radius;
//             
//             // Generate circle points
//             for (var j = 0; j < NumberOfCirclePoints; j++)
//             {
//                  //Generate circle point by rotating the bi-normal vector around the tangent vector
//                  //by a certain degree (step size). This generates points forming a circle around
//                  //the curve point, facing in the direction of the following curve point (tangent direction)
//                  var quaternionRot = Quaternion.AngleAxis(j * _degreeStepSize, tangent);
//                  var rotatedVector = quaternionRot * cpn;
//                  var circlePoint = centerPoint + rotatedVector;
//                  circlePoint *= tubeMeshScalingFactor;
//                  
//                 _tubePoints.Add(circlePoint);
//                 
//                 //yield return null;
//             }            
//             
//             Debug.Log("point_i: " + i);
//             //yield return null;
//             
//         }
//         
//         const MeshTopology topology = MeshTopology.Triangles;
//         List<int> indices;
//
//         // Generate topology indices based on chosen topology
//         switch (topology)
//         {
//             case MeshTopology.Triangles:
//                 StartCoroutine(GenerateCurveMeshTriangleIndices(_tubePoints, true));
//                 
//                 // // Draw triangles twice to cover both sides
//                 // var backIndices = GenerateTriangleIndices(displayVertices, true);
//                 // displayVertices.AddRange(displayVertices);
//                 // indices.AddRange(backIndices);
//                 break;
//             
//         }
//
//         yield return null;
//     }
//
//     private IEnumerator GenerateBottomLidMesh()
//     {
//         var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
//         var firstPoint = curve.WorldPoints[0] * _scalingFactor;
//         var firstCircle = _tubePoints.GetRange(0, NumberOfCirclePoints);
//
//         
//         _bottomLidMesh = GenerateLidMesh(firstPoint, firstCircle);
//         _bottomLidMesh.name = "bottom lid mesh";
//         
//         // Generate game object if none is present
//         _bottomLidGameObject ??= new GameObject("Bottom Lid", new[]
//         {
//             typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider)
//         })
//         {
//             transform =
//             {
//                 parent = transform
//             }
//         };
//
//         _bottomLidGameObject.GetComponent<MeshRenderer>().material = tubeMat;
//
//         // Set mesh
//         _bottomLidGameObject.GetComponent<MeshFilter>().mesh = _bottomLidMesh;
//         
//         // Assign mesh to collider
//         //_bottomLidGameObject.GetComponent<MeshCollider>().sharedMesh = _bottomLidMesh;
//         
//         // Move game object to first curve point
//         //_bottomLidGameObject.transform.position = firstPoint; //_curvePoints[0]; //transform.position;
//         
//         yield return null;
//     }
//
//     private IEnumerator GenerateTopLidMesh()
//     {
//         var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
//         var lastPoint = curve.WorldPoints[curve.WorldPoints.Count - 1] * _scalingFactor;
//         var lastCircle = _tubePoints.GetRange(
//             _tubePoints.Count - NumberOfCirclePoints - 1, NumberOfCirclePoints);
//         
//         _topLidMesh = GenerateLidMesh(lastPoint, lastCircle);
//
//         _topLidMesh.name = "top lid mesh";
//         
//         // Generate game object if none is present
//         _topLidGameObject ??= new GameObject("Top Lid", new[]
//         {
//             typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider)
//         })
//         {
//             transform =
//             {
//                 parent = transform
//             }
//         };
//
//         _topLidGameObject.GetComponent<MeshRenderer>().material = tubeMat;
//
//         // Set mesh
//         _topLidGameObject.GetComponent<MeshFilter>().mesh = _topLidMesh;
//
//         // try
//         // {
//         //     // Assign mesh to collider
//         //     //_topLidGameObject.GetComponent<MeshCollider>().sharedMesh = _topLidMesh;
//         // }
//         // catch (Exception e)
//         // {
//         //     
//         // }
//
//         yield return null;
//     }
//
//     
//
//     private Mesh GenerateLidMesh(Vector3 center, List<Vector3> circlePoints)
//     {
//         var pos = transform.position;
//         var centerPoint = pos + center; // * TubeMeshScalingFactor;
//         
//         var beginningLidPoints = new List<Vector3>();
//         for (var i = 0; i < circlePoints.Count - 1; i++)
//         {
//             // Transform point to local coordinate system since center point is not part of the tube mesh,
//             // i.e. it's position has not been transformed to the local mesh coordinate system
//             var tv2 = pos + circlePoints[i];
//             var tv3 = pos + circlePoints[i + 1];
//             
//             beginningLidPoints.Add(centerPoint);
//             beginningLidPoints.Add(tv2);
//             beginningLidPoints.Add(tv3);
//         }
//
//         var reverseList = beginningLidPoints.AsEnumerable().Reverse();
//         
//         // Add last triangle that connects to the first circle point
//         beginningLidPoints.Add(centerPoint);
//         beginningLidPoints.Add(pos + circlePoints[circlePoints.Count - 1]); //.Last());
//         beginningLidPoints.Add(pos + circlePoints[0]);//.First());
//         
//         beginningLidPoints.AddRange(reverseList);
//         
//         beginningLidPoints.Add(centerPoint);
//         beginningLidPoints.Add(pos + circlePoints[0]);//.First());
//         beginningLidPoints.Add(pos + circlePoints[circlePoints.Count - 1]); //.Last());
//         
//         var beginningLidIndices = GenerateLidTriangleIndices(beginningLidPoints);
//         
//         var bottomLidMesh = new Mesh
//         {
//             vertices = beginningLidPoints.ToArray(),
//         };
//
//         bottomLidMesh.SetIndices(beginningLidIndices, MeshTopology.Triangles, 0);
//         bottomLidMesh.RecalculateNormals();
//         
//         return bottomLidMesh;
//     }
//
//
//     //private List<int> indicesList = new List<int>();
//     
//     private IEnumerator GenerateCurveMeshTriangleIndices(List<Vector3> tubePoints, bool windClockwise)
//     {
//         var indicesList  = new List<int>();
//         for (var i = 0; i < tubePoints.Count - 1; i += NumberOfCirclePoints)
//         {
//             for (var j = 0; j < NumberOfCirclePoints; ++j)
//             {
//                 var baseIndex = i + j;
//
//                 if (baseIndex >= (tubePoints.Count - NumberOfCirclePoints - 1)) continue;
//                 
//                 // Debug.Log(
//                 //     "baseIndex: " + baseIndex +
//                 //     ", baseIndex + 1: " + (baseIndex + 1) +
//                 //     ", baseIndex + Num: " + (baseIndex + numberOfCirclePoints) +
//                 //     ", baseIndex + Num + 1: " + (baseIndex + numberOfCirclePoints + 1)
//                 // );
//                 
//                 Debug.Log("baseIndex: " + i + ", " + j);
//                 
//                 // upper right triangle
//                 indicesList.Add(baseIndex);
//
//                 if (windClockwise)
//                 {
//                     indicesList.Add(baseIndex + NumberOfCirclePoints + 1);
//                     indicesList.Add(baseIndex + NumberOfCirclePoints);
//                 }
//                 else
//                 {
//                     indicesList.Add(baseIndex + NumberOfCirclePoints);
//                     indicesList.Add(baseIndex + NumberOfCirclePoints + 1);    
//                 }
//                 
//                 // lower left triangle
//                 indicesList.Add(baseIndex);
//                 
//                 if (windClockwise)
//                 {
//                     indicesList.Add(baseIndex + 1);
//                     indicesList.Add(baseIndex + NumberOfCirclePoints + 1);
//                 }
//                 else
//                 {
//                     indicesList.Add(baseIndex + NumberOfCirclePoints + 1);
//                     indicesList.Add(baseIndex + 1);
//                 }
//             }
//
//         }
//         
//         _tubeMesh = new Mesh
//         {
//             name= "Tube mesh",
//             vertices = _tubePoints.ToArray()
//         };
//         
//         _tubeMesh.SetIndices(indicesList.ToArray(), MeshTopology.Triangles, 0);
//         
//         _tubeMesh.RecalculateNormals();
//         
//         _meshRenderer.material = tubeMat;
//         
//         // Set mesh
//         _meshFilter.mesh = _tubeMesh;
//         
//         // Assign mesh to collider
//         _meshCollider.sharedMesh = _tubeMesh;
//         
//         StartCoroutine(GenerateBottomLidMesh());
//         //yield return null;
//         
//         StartCoroutine(GenerateTopLidMesh());
//         
//         //return indicesList;
//         
//         yield return null;
//     }
//
//     private List<int> GenerateLidTriangleIndices(List<Vector3> lidPoints)
//     {
//         var indicesList = new List<int>();
//
//         for (var i = 0; i < lidPoints.Count; i++)
//         {
//             indicesList.Add(i);
//         }    
//         
//         return indicesList;
//     }
// }



/// #################################################################################################################


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using Utility;

[RequireComponent(
    typeof(MeshFilter),
    typeof(MeshRenderer),
    typeof(MeshCollider))]
public class TubeMesh : MonoBehaviour
{
    # region Public members
    
    /// <summary>
    /// Material used for the surface of the tube
    /// </summary>
    public Material tubeMat;

    /// <summary>
    /// Material used for the highlighting spheres on the tube
    /// </summary>
    public Material sphereMat;

    /// <summary>
    /// General scaling factor applied to the tube mesh
    /// </summary>
    public float tubeMeshScalingFactor = 1f;

    #endregion Public members
    
    
    #region Private members 
    
    
    private float _sphereScalingFactor = 2f;

    
    /// <summary>
    /// Radius of the final tube
    /// </summary>
    private float _radius = 0.1f;

    /// <summary>
    /// Number of point used to sample the flat circle around every curve point, making up the outside of the tube mesh
    /// </summary>
    private const int NumberOfCirclePoints = 8;

    /// <summary>
    /// Sub-mesh for the outer surface of the tube along the curve
    /// </summary>
    private Mesh _tubeMesh;
    
    /// <summary>
    /// Sub-mesh for the bottom lid of the tube, facing outward
    /// </summary>
    private Mesh _bottomLidMesh;
    
    /// <summary>
    /// Sub-mesh for the top lid of the tube, facing outward
    /// </summary>
    private Mesh _topLidMesh;

    /// <summary>
    /// Number of points making up the mesh
    /// </summary>
    private readonly List<Vector3> _tubePoints = new List<Vector3>();
    
    /// <summary>
    /// Number of spheres highlighting the points on the curve
    /// </summary>
    private readonly List<GameObject> _spheres = new List<GameObject>();
    
    /// <summary>
    /// Game object for the bottom lid mesh
    /// </summary>
    private GameObject _bottomLidGameObject;
    
    /// <summary>
    /// Game object for the top lid mesh
    /// </summary>
    private GameObject _topLidGameObject;

    /// <summary>
    /// Calculated amount of degrees between the points along the circle surrounding each curve point
    /// </summary>
    private float _degreeStepSize;

    /// <summary>
    /// Custom number of sampling points for a curve. Can be used to visualize the difference
    /// in differing sampling counts
    /// </summary>
    private int _numberOfSamplingPoints = -1;
    
    /// <summary>
    /// Signals whether the curve points are visualized along the curve using small spheres
    /// </summary>
    private bool _visualizePoints = false;

    /// <summary>
    /// Cached mesh renderer component
    /// </summary>
    private MeshRenderer _meshRenderer;
    
    /// <summary>
    /// Cached mesh filter component
    /// </summary>
    private MeshFilter _meshFilter;
    
    /// <summary>
    /// Cached mesh collider component
    /// </summary>
    private MeshCollider _meshCollider;

    /// <summary>
    /// Curve scaling factor. Changed by <see cref="SetScalingFactor"/>
    /// </summary>
    private float _scalingFactor = 1f;
    
    private const float CircleDegree = 1f / 360f;

    private List<Vector3> _polyline = new List<Vector3>();
    private List<Vector3> _tanPerpendicularVectors = new List<Vector3>();
    
    #endregion Private members
    
    
    #region Public functions
    
    private void Awake()
    {
        // Cache components
        _meshCollider = GetComponent<MeshCollider>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        // Set tube radius based on scaling factor
        _radius = 0.1f;//0.05f * tubeMeshScalingFactor;
        _sphereScalingFactor = 0.125f * tubeMeshScalingFactor;
        
    }

    public void SetScalingFactor(float val)
    {
        _scalingFactor = val;
        tubeMeshScalingFactor = _scalingFactor;
        _radius = 0.1f; //Mathf.Clamp(0.05f * _scalingFactor, 0.1f, 0.1f);
        _sphereScalingFactor = 0.125f * _scalingFactor;
    }
    
    /// <summary>
    /// Creates the mesh by calculation the topology
    /// </summary>
    public void GenerateFieldMesh(List<Vector3> polyline, List<Vector3> tanPerpendicularVectors)
    {
        _polyline = polyline;
        _tanPerpendicularVectors = tanPerpendicularVectors;
        CalculateMesh();
    }
    
    /// <summary>
    /// Creates the mesh using a given amount of points to sample the curve
    /// </summary>
    /// <param name="sampleCount">Sample count</param>
    public void GenerateFieldMesh(int sampleCount)
    {
        _numberOfSamplingPoints = sampleCount;
        //CalculateMesh();
    }

    #endregion Public functions
    
    /// <summary>
    /// Mesh calculation function
    /// </summary>
    private void CalculateMesh()
    {
        // if(GlobalDataModel.DisplayCurveDatasets == null) return;
        // if(GlobalDataModel.DisplayCurveDatasets.Count == 0) return;

        _tubePoints.Clear();
        _degreeStepSize = 360f / NumberOfCirclePoints;

        StartCoroutine(GenerateCurveMesh());

    }
    
    /// <summary>
    /// Generates the main surface around the curve points
    /// </summary>
    private IEnumerator GenerateCurveMesh()
    {
        //var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var polyline = _polyline; //curve.WorldPoints;
        
        
        // When a sample count was given, sample curve
        // ToDo: Refactor this into second view / subclass
        if(_numberOfSamplingPoints != -1)
        {
            var div = polyline.Count / _numberOfSamplingPoints;
            _degreeStepSize = _numberOfSamplingPoints;

            var newPointList = new List<Vector3>();
            for(var i = 0; i < polyline.Count; i++)
            {
                // Only sample every nth point
                if(i % div == 0)
                {
                    var p = polyline[i];
                    newPointList.Add(p);
                }
            }
            
            foreach(var sphere in _spheres)
            {
                Destroy(sphere);
            }

            _spheres.Clear();
            
            foreach(var point in newPointList)
            {
                var calcPoint = point;
                // if(!curve.Is3DCurve)
                // {
                //     calcPoint = new Vector3(point.x, point.y, 0f);
                // }
                
                calcPoint *= tubeMeshScalingFactor;
                var spherePoint = transform.position + calcPoint;

                if (_visualizePoints)
                {
                    // var sphere = DrawingUtility.DrawSphereScaled(spherePoint, transform, Color.black, 
                    //     _sphereScalingFactor,
                    //     sphereMat);
                    //
                    // sphere.GetComponent<MeshRenderer>().sharedMaterial = sphereMat;
                    //
                    // _spheres.Add(sphere);    
                }
                
            }

            polyline = newPointList;
        }
        
        // Calculate surface mesh points
        for (var i = 0; i < polyline.Count; i++)
        {
            //yield return null;
            
            // Get curve point and direction vectors
            var centerPoint = polyline[i];
            Vector3 tangent; // = curve.FresnetApparatuses[i].Tangent;
            Vector3 biNormal; // = curve.FresnetApparatuses[i].Binormal;
            
            // On first point, generate tangent pointing to next point since
            // we don't have any velocity at the beginning, i.e. the tangent is (0,0,0)
            if (i < polyline.Count - 1)
            {
                tangent = (polyline[i + 1] - centerPoint).normalized;// * radius;
                //biNormal = Vector3.down; //curve.FresnetApparatuses[i + 1].Binormal;
            }
            else
            {
                tangent = (polyline[i - 1] - centerPoint).normalized;
                //biNormal = Vector3.down;
            }

            biNormal = _tanPerpendicularVectors[i];
            
            //yield return null;
            
            // We need a vector to rotate around the curve point to generate a surface
            // around the curve point. For this, we pick the bi-normal of the given
            // curve point.
            // ToDo: Fix edge case where bi-normal is parallel to tangent !
            var perpendicularVec = biNormal;
            var cpn = perpendicularVec.normalized * _radius;
            
            // Generate circle points
            for (var j = 0; j < NumberOfCirclePoints; j++)
            {
                 //Generate circle point by rotating the bi-normal vector around the tangent vector
                 //by a certain degree (step size). This generates points forming a circle around
                 //the curve point, facing in the direction of the following curve point (tangent direction)
                 var quaternionRot = Quaternion.AngleAxis(j * _degreeStepSize, tangent);
                 var rotatedVector = quaternionRot * cpn;
                 var circlePoint = centerPoint + rotatedVector;
                 circlePoint *= tubeMeshScalingFactor;
                 
                _tubePoints.Add(circlePoint);
                
                if(i % 4 == 0)
                    yield return null;
            }            
            
            Debug.Log("point_i: " + i);
            //yield return null;
            
        }
        
        const MeshTopology topology = MeshTopology.Triangles;
        List<int> indices;

        // Generate topology indices based on chosen topology
        switch (topology)
        {
            case MeshTopology.Triangles:
                StartCoroutine(GenerateCurveMeshTriangleIndices(_tubePoints, true));
                
                // // Draw triangles twice to cover both sides
                // var backIndices = GenerateTriangleIndices(displayVertices, true);
                // displayVertices.AddRange(displayVertices);
                // indices.AddRange(backIndices);
                break;
            
        }

        yield return null;
    }

    private IEnumerator GenerateBottomLidMesh()
    {
        //var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var firstPoint = _polyline[0] * _scalingFactor;
        var firstCircle = _tubePoints.GetRange(0, NumberOfCirclePoints);

        
        _bottomLidMesh = GenerateLidMesh(firstPoint, firstCircle);
        _bottomLidMesh.name = "bottom lid mesh";
        
        // Generate game object if none is present
        _bottomLidGameObject ??= new GameObject("Bottom Lid", new[]
        {
            typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider)
        })
        {
            transform =
            {
                parent = transform
            }
        };

        _bottomLidGameObject.GetComponent<MeshRenderer>().material = tubeMat;

        // Set mesh
        _bottomLidGameObject.GetComponent<MeshFilter>().mesh = _bottomLidMesh;
        
        // Assign mesh to collider
        //_bottomLidGameObject.GetComponent<MeshCollider>().sharedMesh = _bottomLidMesh;
        
        // Move game object to first curve point
        //_bottomLidGameObject.transform.position = firstPoint; //_curvePoints[0]; //transform.position;
        
        yield return null;
    }

    private IEnumerator GenerateTopLidMesh()
    {
        //var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var lastPoint = _polyline[_polyline.Count - 1] * _scalingFactor;
        var lastCircle = _tubePoints.GetRange(
            _tubePoints.Count - NumberOfCirclePoints - 1, NumberOfCirclePoints);
        
        _topLidMesh = GenerateLidMesh(lastPoint, lastCircle);

        _topLidMesh.name = "top lid mesh";
        
        // Generate game object if none is present
        _topLidGameObject ??= new GameObject("Top Lid", new[]
        {
            typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider)
        })
        {
            transform =
            {
                parent = transform
            }
        };

        _topLidGameObject.GetComponent<MeshRenderer>().material = tubeMat;

        // Set mesh
        _topLidGameObject.GetComponent<MeshFilter>().mesh = _topLidMesh;

        // try
        // {
        //     // Assign mesh to collider
        //     //_topLidGameObject.GetComponent<MeshCollider>().sharedMesh = _topLidMesh;
        // }
        // catch (Exception e)
        // {
        //     
        // }

        yield return null;
    }

    

    private Mesh GenerateLidMesh(Vector3 center, List<Vector3> circlePoints)
    {
        var pos = transform.position;
        var centerPoint = pos + center; // * TubeMeshScalingFactor;
        
        var beginningLidPoints = new List<Vector3>();
        for (var i = 0; i < circlePoints.Count - 1; i++)
        {
            // Transform point to local coordinate system since center point is not part of the tube mesh,
            // i.e. it's position has not been transformed to the local mesh coordinate system
            var tv2 = pos + circlePoints[i];
            var tv3 = pos + circlePoints[i + 1];
            
            beginningLidPoints.Add(centerPoint);
            beginningLidPoints.Add(tv2);
            beginningLidPoints.Add(tv3);
        }

        var reverseList = beginningLidPoints.AsEnumerable().Reverse();
        
        // Add last triangle that connects to the first circle point
        beginningLidPoints.Add(centerPoint);
        beginningLidPoints.Add(pos + circlePoints[circlePoints.Count - 1]); //.Last());
        beginningLidPoints.Add(pos + circlePoints[0]);//.First());
        
        beginningLidPoints.AddRange(reverseList);
        
        beginningLidPoints.Add(centerPoint);
        beginningLidPoints.Add(pos + circlePoints[0]);//.First());
        beginningLidPoints.Add(pos + circlePoints[circlePoints.Count - 1]); //.Last());
        
        var beginningLidIndices = GenerateLidTriangleIndices(beginningLidPoints);
        
        var bottomLidMesh = new Mesh
        {
            vertices = beginningLidPoints.ToArray(),
        };

        bottomLidMesh.SetIndices(beginningLidIndices, MeshTopology.Triangles, 0);
        bottomLidMesh.RecalculateNormals();
        
        return bottomLidMesh;
    }


    //private List<int> indicesList = new List<int>();
    
    private IEnumerator GenerateCurveMeshTriangleIndices(List<Vector3> tubePoints, bool windClockwise)
    {
        var indicesList  = new List<int>();
        for (var i = 0; i < tubePoints.Count - 1; i += NumberOfCirclePoints)
        {
            for (var j = 0; j < NumberOfCirclePoints; ++j)
            {
                var baseIndex = i + j;

                if (baseIndex >= (tubePoints.Count - NumberOfCirclePoints - 1)) continue;
                
                // Debug.Log(
                //     "baseIndex: " + baseIndex +
                //     ", baseIndex + 1: " + (baseIndex + 1) +
                //     ", baseIndex + Num: " + (baseIndex + numberOfCirclePoints) +
                //     ", baseIndex + Num + 1: " + (baseIndex + numberOfCirclePoints + 1)
                // );
                
                Debug.Log("baseIndex: " + i + ", " + j);
                
                // upper right triangle
                indicesList.Add(baseIndex);

                if (windClockwise)
                {
                    indicesList.Add(baseIndex + NumberOfCirclePoints + 1);
                    indicesList.Add(baseIndex + NumberOfCirclePoints);
                }
                else
                {
                    indicesList.Add(baseIndex + NumberOfCirclePoints);
                    indicesList.Add(baseIndex + NumberOfCirclePoints + 1);    
                }
                
                // lower left triangle
                indicesList.Add(baseIndex);
                
                if (windClockwise)
                {
                    indicesList.Add(baseIndex + 1);
                    indicesList.Add(baseIndex + NumberOfCirclePoints + 1);
                }
                else
                {
                    indicesList.Add(baseIndex + NumberOfCirclePoints + 1);
                    indicesList.Add(baseIndex + 1);
                }
            }

        }
        
        _tubeMesh = new Mesh
        {
            name= "Tube mesh",
            vertices = _tubePoints.ToArray()
        };
        
        _tubeMesh.SetIndices(indicesList.ToArray(), MeshTopology.Triangles, 0);
        
        _tubeMesh.RecalculateNormals();
        
        _meshRenderer.material = tubeMat;
        
        // Set mesh
        _meshFilter.mesh = _tubeMesh;
        
        // Assign mesh to collider
        _meshCollider.sharedMesh = _tubeMesh;
        
        StartCoroutine(GenerateBottomLidMesh());
        //yield return null;
        
        StartCoroutine(GenerateTopLidMesh());
        
        //return indicesList;
        
        yield return null;
    }

    private List<int> GenerateLidTriangleIndices(List<Vector3> lidPoints)
    {
        var indicesList = new List<int>();

        for (var i = 0; i < lidPoints.Count; i++)
        {
            indicesList.Add(i);
        }    
        
        return indicesList;
    }
}
