using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlacement : MonoBehaviour
{
    public GameObject PointerObject;
    public Material SpawnPointMat;

    private GameObject sphere;

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);

        if (sphere != null)
        {
            Destroy(sphere);
        }

        if (collision.gameObject != PointerObject.gameObject)
        {
            // Debug.Log("collObjName: " + collision.gameObject.name +
            //           ", pointerObjName: " + PointerObject.name);
            //
            return;
        }

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(contact.point.x, transform.position.y + 0.025f, contact.point.z);
        
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //Instantiate(new SphereCollider)
        sphere.GetComponent<MeshRenderer>().sharedMaterial = SpawnPointMat;

        Debug.Log("contact point: " + contact.point);

        var transformedPoint = transform.InverseTransformPoint(contact.point);
        
        Debug.Log("transformed point: " + transformedPoint);

        // var x = transformedPoint.x;
        // var y = transformedPoint.y;
        // var z = transformedPoint.z;

        //x += 0.5f;
        //y += 0.5f;

        var finalPoint = transformedPoint;
        
        Debug.Log("final point: " + finalPoint);
        
        sphere.name = "TravelTarget";

        var closestPoint = Physics.ClosestPoint(contact.point, collision.collider, collision.collider.transform.position,
            collision.collider.transform.rotation);

        GlobalDataModel.ClosestPointOnMesh = transformedPoint; //closestPoint;

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
