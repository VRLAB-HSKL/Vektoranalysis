using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Static utility class containing frequently used functions related to drawing simple primitives
    /// and gizmos in the unity scene
    /// </summary>
    public static class DrawingUtility
    {
        public static GameObject DrawSphereOnLine(GameObject line, int pointIndex, Vector3 bbScale)
        {
            var obj = line;
            var lr = obj.GetComponent<LineRenderer>();
            var linePoints = new Vector3[lr.positionCount];
            lr.GetPositions(linePoints);

            var point = linePoints[pointIndex];

            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.name = "Sphere_" + line.name + "_" + pointIndex;
            sphere.transform.parent = obj.transform;

            // Scale sphere to about 5% the size of the bounding box
            //var bbScale = BoundingBox.transform.localScale;
            var maxScaleFactor = Mathf.Max(bbScale.x, bbScale.y, bbScale.z);
            var maxVector = new Vector3(maxScaleFactor, maxScaleFactor, maxScaleFactor);
            // float divX = 1f / maxScale;
            // float divY = 1f / maxScale;
            // float divZ = 1f / maxScale;
            // var divScale = new Vector3(divX, divY, divZ);
            var newScale = maxVector * 0.025f; // * 1f; //0.05f;
            
            sphere.transform.localScale = Vector3.Scale(Vector3.one, newScale);
                                    
            // Debug.Log("bbScale: " + bbScale + ", maxScale: " + maxVector +  
            //           // ", divValues: " + divX + " " + divY + " " + divZ +
            //           // ", divScale: " + divScale.x + " " + divScale.y + " " + divScale.z +
            //          ", newScale: " + newScale.x + " " + newScale.y + " " + newScale.z);
            
            sphere.GetComponent<MeshRenderer>().material.color = Color.red;

            sphere.transform.position = point;

            return sphere;
        }

        public static GameObject DrawArrowOnLine(GameObject line, int pointIndex, 
            GameObject arrowPrefab, Vector3 direction,
            Vector3 bbScale)
        {
            var obj = line;
            var lr = obj.GetComponent<LineRenderer>();
            var linePoints = new Vector3[lr.positionCount];
            lr.GetPositions(linePoints);

            var point = linePoints[pointIndex];

            var arrow = GameObject.Instantiate(arrowPrefab, obj.transform);
            arrow.name = "Arrow_" + line.name + "_" + pointIndex;
            arrow.transform.position = point;
            //arrow.GetComponent<ArrowController>().PointTowards(direction);

            // Scale arrow to about 10% the size of the bounding box
            //var bbScale = BoundingBox.transform.localScale;
            var maxScale = Mathf.Max(bbScale.x, bbScale.y, bbScale.z);
            // var newScale = new Vector3(1f / maxScale, 1f / maxScale, 1f / maxScale);
            var maxVector = new Vector3(maxScale, maxScale, maxScale);
            var newScale = maxVector * 0.05f;
            arrow.transform.localScale = Vector3.Scale(Vector3.one, newScale); //newScale;
            
            var lookPoint = point + direction;
            
            //Debug.Log("Point: " + point + ", LookPoint: " + lookPoint);
            arrow.transform.LookAt(lookPoint);
            
            return arrow;
        }

        public static void DrawSphere(Vector3 point, Transform parent, Color color, Vector3 bbScale)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Sphere_point_" + point;
            sphere.transform.parent = parent;

            //var bbScale = BoundingBox.transform.localScale;
            var maxScaleFactor = Mathf.Max(bbScale.x, bbScale.y, bbScale.z);
            var maxVector = new Vector3(maxScaleFactor, maxScaleFactor, maxScaleFactor);
            // float divX = 1f / maxScale;
            // float divY = 1f / maxScale;
            // float divZ = 1f / maxScale;
            // var divScale = new Vector3(divX, divY, divZ);
            var newScale = maxVector * 0.025f; // * 1f; //0.05f;
            
            sphere.transform.localScale = Vector3.Scale(Vector3.one, newScale);
                                    
            sphere.GetComponent<MeshRenderer>().material.color = color;

            sphere.transform.position = point;
        }

        public static void DrawArrow(Vector3 start, Vector3 target, Transform parent, GameObject ArrowPrefab, Vector3 bbScale)
        {
            var arrow = Object.Instantiate(ArrowPrefab, parent);
            arrow.name = "Arrow_" + start + "_to_" + target;
            arrow.transform.position = start; //Vector3.Lerp(start, target, 0.5f);
            //arrow.GetComponent<ArrowController>().PointTowards(direction);

            // Scale arrow to about 10% the size of the bounding box
            //var bbScale = BoundingBox.transform.localScale;
            var maxScale = Mathf.Max(bbScale.x, bbScale.y, bbScale.z);
            // var newScale = new Vector3(1f / maxScale, 1f / maxScale, 1f / maxScale);
            var maxVector = new Vector3(maxScale, maxScale, maxScale);
            var newScale = maxVector * 0.05f;
            arrow.transform.localScale = Vector3.Scale(Vector3.one, newScale); //newScale;

            var scale = arrow.transform.localScale;
            var zScale = Vector3.Distance(start, target);

            arrow.transform.localScale = new Vector3(scale.x, scale.y, zScale);
            
            //var direction = target - start;
            
            //ToDo: Scale arrow length wise based on distance
            // var dirMag = direction.magnitude;
            // arrow.transform.localScale = Vector3.Scale(arrow.transform.localScale, new Vector3(1f, 1f, dirMag));

            //var centerPos = (start + target) * 0.5f;
            
            //arrow.transform.localScale = Vector3.Scale(arrow.transform.localScale, new Vector3(1f, 1f, scaleZ));

            //var lookPoint = start + direction;
            arrow.transform.LookAt(target);
        }
        
        public static void DrawPath(List<Vector3> points, Transform parent, GameObject arrowPrefab, Vector3 bbScale)
        {
            if (points.Count == 0) return;

            var path = new GameObject
            {
                transform =
                {
                    parent = parent
                },
                name = "Path_" + points[0]
            };

            //Debug.Log("Creating empty path parent");
            Object.Instantiate(path);

            if (points.Count == 1)
            {
                DrawSphere(points[0], path.transform, Color.red, bbScale);
            }
            else
            {
                path.name += "_to_" + points[points.Count - 1];

                var stepSize = 1f / points.Count;
                
                for (var i = 0; i < points.Count; i++)
                {
                    var currStep = i * stepSize;
                    var color = new Color(1f - (currStep), currStep, 0f);
                        
                    Debug.Log(i + " - " + color);
                    
                    DrawSphere(points[i], path.transform, color, bbScale);
                    
                    // Draw arrow for every element except for the last one, because there is no next point to connect to
                    if(i < points.Count - 1)
                        DrawArrow(points[i], points[i + 1], path.transform,arrowPrefab, bbScale);
                }
            }
            
        }
    }
}