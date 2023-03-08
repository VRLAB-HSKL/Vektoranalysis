using Model.ScriptableObjects;
using UnityEngine;
using Utility;

namespace Travel
{
    public class MapPlacement : MonoBehaviour
    {
        public ScalarFieldManager ScalarFieldManager;
        public TravelManager TravelManager;
        
        public GameObject PointerObject;
        public Material SpawnPointMat;

        private GameObject sphere;

        private MeshRenderer _mr;
        
        private void Start()
        {
            _mr = GetComponent<MeshRenderer>();
            SetTexture();
        }

        public void SetTexture()
        {
            _mr.material.mainTexture = ScalarFieldManager.CurrentField.MeshTexture; //texture;
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            var contact = collision.GetContact(0);
        
            if (collision.gameObject != PointerObject.gameObject)
                return;
            
            if (sphere != null)
            {
                Destroy(sphere);
            }

            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(contact.point.x, transform.position.y + 0.025f, contact.point.z);
            sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            //Instantiate(new SphereCollider)
            sphere.GetComponent<MeshRenderer>().sharedMaterial = SpawnPointMat;
            sphere.name = "TravelTarget";
            //sphere.transform.parent = transform;

            var transformedPoint = transform.InverseTransformPoint(contact.point);
            
            
            var horizontalCoordinate = transformedPoint.z;
            var verticalCoordinate = transformedPoint.x;

            var xRangeMin = ScalarFieldManager.CurrentField.ParameterRangeX.Item1; 
            var xRangeMax = ScalarFieldManager.CurrentField.ParameterRangeX.Item2; 
            
            var yRangeMin = ScalarFieldManager.CurrentField.ParameterRangeY.Item1;
            var yRangeMax = ScalarFieldManager.CurrentField.ParameterRangeY.Item2;

            var mappedX = CalcUtility.MapValueToRange(horizontalCoordinate, -0.5f, 0.5f, xRangeMin, xRangeMax);
            var mappedY = CalcUtility.MapValueToRange(verticalCoordinate, -0.5f, 0.5f, yRangeMin, yRangeMax);
        
            var closestPointIndex = int.MaxValue;
            var minDist = float.MaxValue;
            for(var i = 0; i < ScalarFieldManager.CurrentField.RawPoints.Count; i++)
            {
                var point = ScalarFieldManager.CurrentField.RawPoints[i];
                var dist = Vector3.Distance(point, new Vector3(mappedX, mappedY));
                
                if (minDist > dist)
                {
                    minDist = dist;
                    closestPointIndex = i;
                }
            }

            TravelManager.estimatedIndex = closestPointIndex;
        
            Debug.Log("contact point: " + contact.point + 
                      ", mapped values: (" + mappedX + ", " + mappedY + ")" + 
                      ", ParamX(" + xRangeMin + ", " + xRangeMax + ")" +
                      ", ParamY(" + yRangeMin + ", " + yRangeMax + ")" +
                      ", estimated index: " + TravelManager.estimatedIndex);
        }
    }
}
