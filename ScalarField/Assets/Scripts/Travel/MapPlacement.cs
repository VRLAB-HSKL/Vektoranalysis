using Model;
using UnityEngine;
using Utility;

namespace Travel
{
    public class MapPlacement : MonoBehaviour
    {
        public GameObject PointerObject;
        public Material SpawnPointMat;

        private GameObject sphere;

        private void Start()
        {
            var mr = GetComponent<MeshRenderer>();

            var cm = GlobalDataModel.CurrentField.ColorMapId;
            var dataClassesCount = GlobalDataModel.CurrentField.ColorMapDataClassesCount;
            // var texture = Resources.Load(
            //     "texture_maps/" + cm + "/" + dataClassesCount + "/" +
            //     cm + "_" + dataClassesCount + "_texture.png"
            // ) as Texture2D;

            mr.material.mainTexture = GlobalDataModel.CurrentField.MeshTexture; //texture;
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            var contact = collision.GetContact(0);
        
            if (collision.gameObject != PointerObject.gameObject)
            {
                // Debug.Log("collObjName: " + collision.gameObject.name +
                //           ", pointerObjName: " + PointerObject.name);
            
                return;
            }

            if (sphere != null)
            {
                Destroy(sphere);
            }

            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(contact.point.x, transform.position.y + 0.025f, contact.point.z);
        
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            //Instantiate(new SphereCollider)
            sphere.GetComponent<MeshRenderer>().sharedMaterial = SpawnPointMat;

        
            sphere.name = "TravelTarget";
        

            var horizontalCoordinate = contact.point.z;
            var verticalCoordinate = contact.point.x;

            var xRangeMin = GlobalDataModel.CurrentField.ParameterRangeX.Item1; //GlobalDataModel.InitFile.points.Min(p => p[0]);
            var xRangeMax = GlobalDataModel.CurrentField.ParameterRangeX.Item2; //GlobalDataModel.InitFile.points.Max(p => p[0]);

            var yRangeMin = GlobalDataModel.CurrentField.ParameterRangeY.Item1;
            var yRangeMax = GlobalDataModel.CurrentField.ParameterRangeY.Item2;

            var mappedX = CalcUtility.MapValueToRange(horizontalCoordinate, -0.5f, 0.5f, xRangeMin, xRangeMax);
        
            var mappedY = CalcUtility.MapValueToRange(verticalCoordinate, -0.5f, 0.5f, yRangeMin, yRangeMax);
        
            // var mappedX = CalcUtility.MapRange(x, )
            //
            // var tmp = CalcUtility.MapRange()

        
        
            var transformedPoint = transform.InverseTransformPoint(contact.point);
        
        
        

            // var finalIndex = 0;
        
            var closest = int.MaxValue;
            var minDifference = int.MaxValue;
            for(var i = 0; i < GlobalDataModel.CurrentField.MeshPoints.Count; i++)
            {
                var point = GlobalDataModel.CurrentField.MeshPoints[i];
                var differenceX = Mathf.Abs(point[0] - mappedX);
                var differenceY = Mathf.Abs(point[1] - mappedY);

                var difference = differenceX + differenceY;
            
                if (minDifference > difference)
                {
                    minDifference = (int)difference;
                    closest = i;
                }
            }

            GlobalDataModel.EstimatedIndex = closest;
        
            Debug.Log("contact point: " + contact.point + ", transformed point: " + transformedPoint +
                      ", mapped values: (" + mappedX + ", " + mappedY + ")" + 
                      ", estimated index: " + GlobalDataModel.EstimatedIndex);
        
            //Debug.Log();

            // var x = transformedPoint.x;
            // var y = transformedPoint.y;
            // var z = transformedPoint.z;

            //x += 0.5f;
            //y += 0.5f;

            //var finalPoint = transformedPoint;
        
            //Debug.Log("final point: " + finalPoint);
        
        

            //var closestPoint = Physics.ClosestPoint(contact.point, collision.collider, collision.collider.transform.position,
            //    collision.collider.transform.rotation);

            //GlobalDataModel.ClosestPointOnMesh = transformedPoint; //closestPoint;

            // if(Physics.Raycast(new Ray(contact.point, Vector3.down), out RaycastHit hit))
            // {
            //     if (hit.collider is MeshCollider)
            //     {
            //         Debug.Log("Raycast Hit!");
            //         var collider = hit.collider as MeshCollider;
            //         var mesh = collider.sharedMesh;
            //         
            //         Debug.Log("Triangle: " + hit.triangleIndex + " / " + mesh.triangles.Length);
            //         var p0 = mesh.vertices[mesh.triangles[hit.triangleIndex]];
            //         var p1 = mesh.vertices[mesh.triangles[hit.triangleIndex] + 1];
            //         var p2 = mesh.vertices[mesh.triangles[hit.triangleIndex] + 2];
            //
            //         var p = p0.y > p1.y ? p0 : p1;
            //         p = p2.y > p.y ? p2 : p;
            //             
            //         //GlobalDataModel.ClosestPointOnMesh = hit.collider.transform.TransformPoint(p);
            //         GlobalDataModel.EstimatedIndex = mesh.triangles[hit.triangleIndex];
            //     }
            // }

            // var mesh = GetComponent<MeshFilter>().mesh;
            // for (int i = 0; i < mesh.vertices.Length; i++)
            // {
            //     var point = mesh.vertices[i];
            //     var pointVec = transform.TransformPoint(point); //new Vector3(point.x, point.y, point.z);
            //     if (pointVec == contact.point)
            //     {
            //         Debug.Log("Collided point: " + contact.point);
            //         break;
            //     }
            // }

        }
    }
}
