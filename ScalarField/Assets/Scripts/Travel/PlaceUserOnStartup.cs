using System.Linq;
using Model;
using Model.ScriptableObjects;
using UnityEngine;
using Utility;

namespace Travel
{
    /// <summary>
    /// Places the user at a specified position on entering the scene
    /// </summary>
    public class PlaceUserOnStartup : MonoBehaviour
    {
        public ScalarFieldManager ScalarFieldManager;
        public TravelManager TravelManager;
        
        public MeshFilter field;
        public GameObject Cockpit;
    
        /// <summary>
        /// Positional offset, initially used to place the cockpit below the player
        /// </summary>
        public Vector3 CockpitOffset = new Vector3(0f, -2f, 0f);

        public bool SpawnAtEagle;
    
        private GameObject user;
        private Vector3 MainPointPosition = Vector3.zero;
    
        // Start is called before the first frame update
        void Start()
        {
            user = GameObject.Find("ViveRig");

            if (user is null) return;

            var estimatedIndex = TravelManager.EstimatedIndex;
            var initValues = ScalarFieldManager.CurrentField.MeshPoints[estimatedIndex];

            var initVector = new Vector3(initValues[0], initValues[1], initValues[2]);

            var xMin = ScalarFieldManager.CurrentField.ParameterRangeX.Item1;// InitFile.Info.x_param_range[0];
            var xMax = ScalarFieldManager.CurrentField.ParameterRangeX.Item2; // InitFile.Info.x_param_range[1];

            var yMin = ScalarFieldManager.CurrentField.ParameterRangeY.Item1; // InitFile.Info.y_param_range[0];
            var yMax = ScalarFieldManager.CurrentField.ParameterRangeY.Item2; // InitFile.Info.y_param_range[1];
        
            var zMin = ScalarFieldManager.CurrentField.MeshPoints.Min(p => p[2]);
            var zMax = ScalarFieldManager.CurrentField.MeshPoints.Max(p => p[2]);
        
            var mappedX = CalcUtility.MapValueToRange(initValues[0], xMin, xMax, 0f, 1f);
            var mappedY = CalcUtility.MapValueToRange(initValues[1], yMin, yMax, 0f, 1f);
        
            //ToDo: Map z values based on vertical bounding box, useful for curves like banana
            var mappedZ = CalcUtility.MapValueToRange(initValues[2], zMin, zMax, 0f, 10f);
        
            // Flip sign on x coordinate
            //var initVector = new Vector3(-initValues[0], initValues[1], initValues[2]);

            var mappedVector = new Vector3(mappedX, mappedY, mappedZ);
        
            var boundsSize = field.mesh.bounds.size;
            var extentsSize = field.mesh.bounds.extents;
            var finalPoint = new Vector3(
                mappedVector.x * extentsSize.x, //finalPoint.z * boundsSize.x, 
                mappedVector.z, // Re-flip y and z coordinates
                mappedVector.y * extentsSize.z //finalPoint.x * boundsSize.z);
            ); 
        
            // Adjust vertical cockpit position based on raycast hit
            var finalY = 2f;
            var hitPoint = Vector3.zero;
        
            // Shoot down and up in hopes of hitting the mesh and positioning the player above the hit point
            if (Physics.Raycast(new Ray(finalPoint, Vector3.down), out RaycastHit hitDown))
            {
                finalY += hitDown.point.y;
                //Debug.Log("DownHit: " + finalY);
            }
            else if (Physics.Raycast(new Ray(finalPoint, Vector3.up), out RaycastHit hitUp))
            {
                finalY += hitUp.point.y;
            
                //Debug.Log("UpHit: " + finalY);
            }
        
            //finalPoint = new Vector3(finalPoint.x, finalY, finalPoint.z); 
        
        
            // Debug.Log("initVector: " + initVector + 
            //           ", mapped vector: " + mappedVector + 
            //           "Placing at final point: " + finalPoint);
            //
            // Debug.Log("MeshBounds size x y z: " + boundsSize.x + " " + boundsSize.y + " " + boundsSize.z +
            //           ", MeshBounds extents x y z: " + extentsSize.x + " " 
            //           + extentsSize.y + " " + extentsSize.z);


            if (SpawnAtEagle)
            {
                var eaglePoint = GameObject.Find("EaglePoint");
                finalPoint = eaglePoint.transform.position;
                user.transform.position = finalPoint;
                user.transform.rotation = eaglePoint.transform.rotation;
            
                Cockpit.transform.position = finalPoint;
                Cockpit.transform.rotation = eaglePoint.transform.rotation;
            }
            else
            {
                user.transform.position = finalPoint;
                Cockpit.transform.position = finalPoint; // + CockpitOffset;    
            }
        
            //DrawMainPoint();
            //DrawArrow(Vector3.up, 10f);
        
        }

        private void DrawMainPoint()
        {
            if (Physics.Raycast(new Ray(user.transform.position, Vector3.down), out RaycastHit hitDown))
            {
                Debug.Log("MainPointHit");

                MainPointPosition = hitDown.point;
            
                var pointMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                pointMarker.transform.position = MainPointPosition;

                //pointMarker.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                pointMarker.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 0f);
            }
        }

        private void DrawArrow(Vector3 direction, float length = 1f)
        {
            var arrowObj = new GameObject("Arrow_" + direction);
            arrowObj.transform.parent = this.gameObject.transform;

            var pointCount = 5;
            var points = new Vector3[pointCount]; //List<Vector3>();
            var step = length / pointCount;
        
            for(var i = 0; i < pointCount; i++)
                points[i] = MainPointPosition + i * step * direction;
        
        
            var lr = arrowObj.AddComponent<LineRenderer>();
            lr.SetPositions(points);
            lr.material.color = Color.red;
            lr.widthMultiplier = 0.1f;
        
        

            //var arrowObj2 = Instantiate(arrowObj, this.transform);
            //arrowObj2.transform.position = MainPointPosition; // + direction;

        }
    }
}
