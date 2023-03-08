using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using Utility;

namespace ProceduralMesh
{
    /// <summary>
    /// Script to generate a simple unity mesh topology and texturing to visualize a scalar field in the scene.
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SimpleProceduralMesh : MonoBehaviour
    {
        public ScalarFieldManager ScalarFieldManager;
        
        // public Transform RenderingTarget;
        // public Vector3 PositionOffset;
        // public Vector3 ScalingVector = Vector3.one;
        public GameObject BoundingBox;
        
        public bool PositionMeshAtOrigin;

        private bool _isMeshTransparent;

        public void ToogleMeshTransparency()
        {
            _isMeshTransparent = !_isMeshTransparent;
            
            var attenuation = _isMeshTransparent ? 0.25f : 1f;
            SetMeshTransparency(attenuation);   
        }
        
        private void SetMeshTransparency(float attenuation)
        {
            // var mr = GetComponent<MeshRenderer>();
            // var mat = mr.material;
            // mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, attenuation);
            // mr.material = mat;

            
            GetComponent<MeshRenderer>().material.mainTexture = 
                attenuation < 1f ? 
                    ScalarFieldManager.TransparentTexture : 
                    ScalarFieldManager.CurrentField.MeshTexture;
        }
        
        
        private void Start()
        {
            var mesh = MeshUtility.GenerateFieldMesh(ScalarFieldManager.CurrentField, BoundingBox);
            //mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
            
            // Set mesh
            GetComponent<MeshFilter>().mesh = mesh;
            
            // Assign mesh to collider
            var meshCollider = GetComponent<MeshCollider>();
            //collider.convex = true;
            meshCollider.sharedMesh = mesh;
            
        
            if (PositionMeshAtOrigin)
            {
                PositionMeshCenterAtOrigin();
            }   
        
            var mat = GetComponent<MeshRenderer>().material;
            //mat.color = new Color(r: 0.75f, g: 0.75f, b: 0.75f, a: 1f);
            mat.mainTexture = ScalarFieldManager.CurrentField.MeshTexture;
            // var texture = Resources.Load<Texture2D>("texture_maps/test/coolwarm");
            // texture.filterMode = FilterMode.Bilinear;
            // texture.wrapModeU = TextureWrapMode.MirrorOnce;

            //mat.mainTexture = texture;
        }
    
        
    
    
        /// <summary>
        /// Reposition the center of the mesh to the origin point of the scene
        /// </summary>
        private void PositionMeshCenterAtOrigin()
        {
            var tmp = transform.position - GetComponent<MeshRenderer>().bounds.center;
            //var tmp = BoundingBox.GetComponent<MeshRenderer>().bounds.center;
            //Debug.Log("MeshPositioningVector: " + tmp);
            //transform.position += tmp;
            transform.parent.position = tmp;
        }
    
        
    
    }
}
