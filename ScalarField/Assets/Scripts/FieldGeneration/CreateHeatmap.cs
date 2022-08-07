using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using Utility;

namespace FieldGeneration
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class CreateHeatmap : MonoBehaviour
    {
        public GameObject BoundingBox;
        public bool ShowOnStartup;
    
        private MeshRenderer _mr;
        private Bounds _bb;
    
        public void ToggleVisibility()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    
        // Start is called before the first frame update
        private void Start()
        {
            _mr = GetComponent<MeshRenderer>();
            _mr.material.mainTexture = GlobalDataModel.CurrentField.MeshTexture;
            _bb = BoundingBox.GetComponent<MeshRenderer>().bounds;

            var cubePointList = new List<Vector3>();
            var mf = GetComponent<MeshFilter>();
            mf.mesh.GetVertices(cubePointList);
            var scaledPointList = new List<Vector3>();

            var xMin = cubePointList.Min(p => p.x);
            var yMin = cubePointList.Min(p => p.y);
            var zMin = cubePointList.Min(p => p.z);
        
            var xMax = cubePointList.Max(p => p.x);
            var yMax = cubePointList.Max(p => p.y);
            var zMax = cubePointList.Max(p => p.z);

            var minVector = new Vector3(xMin, yMin, zMin);
            var maxVector = new Vector3(xMax, yMax, zMax);
        
            foreach (var point in cubePointList)
            {
                var oldY = point.y;
                var scaledPoint = CalcUtility.MapVectorToRange(point, minVector, maxVector,
                    -_bb.extents, _bb.extents);
                var finalPoint = new Vector3(scaledPoint.x, oldY, scaledPoint.z);
                scaledPointList.Add(finalPoint);
                //Debug.Log("initPoint: " + point + ", finalPoint: " + finalPoint);
            }

            mf.mesh.SetVertices(scaledPointList);
        
            // Hide heatmap initially
            gameObject.SetActive(ShowOnStartup);
        }

    }
}
